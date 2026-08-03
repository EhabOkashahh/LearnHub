using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Courses;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Presistence.Interceptors
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if(context is null) return ValueTask.FromResult(result);

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
            var entityType = context.Model.FindEntityType(type);
            foreach(var nav in entityType.GetNavigations())
            {
                if(nav.PropertyInfo?.GetCustomAttributes(typeof(CascadeSoftDeleteAttribute),false).Length == 0) continue;

                var fkName = nav.ForeignKey.Properties[0].Name;
                var ChildType = nav.TargetEntityType.ClrType;
                var parentIds = visited[type];

            }
        }

        private static Expression<Func<T,bool>> IdIn<T>(HashSet<object> IdsSet, string fk)
        {
            var param = Expression.Parameter(typeof(T), "p"); // x => (x == parent type)
            var prop = Expression.Call(typeof(EF), nameof(EF.Property), [typeof(object)],param , Expression.Constant(fk)); // EF.Property<object>(x,"fk")
            var contains = Expression.Call(typeof(HashSet<object>).GetMethod(nameof(HashSet<object>.Contains))!,Expression.Constant(IdsSet),prop);

            return Expression.Lambda<Func<T,bool>>(contains,param);
        }
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
                set.Add(id);
            }
        }
    }
}