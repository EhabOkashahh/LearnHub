using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;

namespace Services.Specifications
{
    public class Specifications<TKey, TEntity> : ISpecifications<TKey, TEntity> where TEntity : BaseEntity<TKey>
    {
        public Specifications(Expression<Func<TEntity, bool>>? Expression)
        {
            Criteria = Expression;
        }
        public List<Expression<Func<TEntity, object>>> IncludeExpression { get; set; } = new List<Expression<Func<TEntity, object>>>();

        public List<Func<IQueryable<TEntity>,IQueryable<TEntity>>> IncludeAction { get; set; } = new();
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }
        public Expression<Func<TEntity, object>>? OrderByAsc { get; set; }
        public Expression<Func<TEntity, object>>? OrderByDesc { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginated { get;  set; }



        
        
        
        public void ApplyPagination(int? PageIndex, int? PageSize)
        {
            if (PageIndex.HasValue && PageSize.HasValue)
            {
                Skip = (PageIndex.Value - 1) * PageSize.Value;
                Take = PageSize.Value;
                IsPaginated = true;
            }
        }
        public void AddOrderByAsc(Expression<Func<TEntity, object>> orderByAsc)
        {
            OrderByAsc = orderByAsc;
        }
        public void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDesc)
        {
            OrderByDesc = orderByDesc;
        }

        public void AddInclude(Func<IQueryable<TEntity>,IQueryable<TEntity>> action)
        {
            IncludeAction.Add(action);
        }
    }
}