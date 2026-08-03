using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Courses;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Presistence.Interceptors
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if(context is null) return await ValueTask.FromResult(result);

            var visited = new Dictionary<Type,HashSet<object>>();
            var roots = context.ChangeTracker.Entries().Where(e => e.Entity is ISoftDeletable entity 
                                                                && entity.IsDeleted && e.State == EntityState.Modified
                                                                && e.Property(nameof(ISoftDeletable.IsDeleted)).IsModified).Select(e => e.Entity).ToList();
            Add(visited,roots,context);

            var currentLevel = visited.Keys.ToList();

            while(currentLevel.Count > 0)
            {
                var nextLevel = new List<Type>();
                foreach(var type in currentLevel)
                    await CascadeLevelAsync(context,type,visited,nextLevel,cancellationToken);
                currentLevel = nextLevel;
            }
            
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private async Task CascadeLevelAsync(DbContext context, Type type, Dictionary<Type, HashSet<object>> visited, List<Type> nextLevel, CancellationToken cancellationToken)
        {
            var entityType = context.Model.FindEntityType(type)!;
            foreach(var nav in entityType.GetNavigations())
            {
                if(nav.PropertyInfo?.GetCustomAttributes(typeof(CascadeSoftDeleteAttribute),false).Length == 0) continue;

                var fkName = nav.ForeignKey.Properties[0].Name;
                var fkType = nav.ForeignKey.Properties[0].ClrType;
                var ChildType = nav.TargetEntityType.ClrType;
                var parentIds = visited[type];
                if(parentIds.Count == 0) continue;

                var predicate = IdIn(ChildType, fkType, parentIds, fkName); 

                var setMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), Type.EmptyTypes)!;
                var genericSetMethod = setMethod.MakeGenericMethod(ChildType);

                var dbSet = genericSetMethod.Invoke(context, null);

                var query = ((IQueryable)dbSet!).Provider.CreateQuery(
                Expression.Call(typeof(Queryable), nameof(Queryable.Where), new[] { ChildType }, ((IQueryable)dbSet).Expression, predicate));

                var toListMethod = typeof(EntityFrameworkQueryableExtensions).GetMethods()
                                   .Single(m => m.Name == nameof(EntityFrameworkQueryableExtensions.ToListAsync)
                                    && m.IsGenericMethod && m.GetParameters().Length == 2);

                var GenericToList = toListMethod.MakeGenericMethod(ChildType);

                var task = (Task)GenericToList.Invoke(null,new object[] {query , cancellationToken})!;
                await task;

                var result = task.GetType().GetProperty("Result")!.GetValue(task);

                foreach (object child in (IEnumerable)result!)     
                {
                    if (child is ISoftDeletable sd)
                    {
                        sd.IsDeleted = true;
                        sd.DeletedAt = DateTime.UtcNow;
                    }
                    
                    var childId = context.Entry(child).Property("Id").CurrentValue;

                    if (!visited.TryGetValue(ChildType, out var childSet))
                    {
                        childSet = new HashSet<object>();
                        visited[ChildType] = childSet;
                    }
                    childSet.Add(childId!);
                }

                if(visited[ChildType].Count > 0 && !nextLevel.Contains(ChildType)) nextLevel.Add(ChildType);
            }
        }

        private static LambdaExpression IdIn(Type childType, Type keyType, IEnumerable<object> ids, string fk)
        {
            var param = Expression.Parameter(childType, "p"); 
            var prop = Expression.Property(param, fk);
            var containsMethod = typeof(Enumerable).GetMethods()
                .Single(m => m.Name == nameof(Enumerable.Contains) && m.IsGenericMethod && m.GetParameters().Length == 2)
                .MakeGenericMethod(keyType);
            var contains = Expression.Call(containsMethod, Expression.Constant(CastIds(ids, keyType)), prop);

            return Expression.Lambda(contains, param);
        }
        private static object CastIds(IEnumerable<object> ids, Type keyType) =>
            typeof(Enumerable).GetMethod(nameof(Enumerable.Cast))!.MakeGenericMethod(keyType)
                .Invoke(null, new object[] { ids })!;
        private static void Add(Dictionary<Type, HashSet<object>> visited, List<object> roots , DbContext context)
        {
            foreach(var root in roots)
            {                
                var type = root.GetType();
                var id = context.Entry(root).Property("Id").CurrentValue;
                if (!visited.TryGetValue(type, out var set))
                {
                    set = new HashSet<object>();
                    visited[type] = set;
                }
                set.Add(id!);
            }
        }
    }
}