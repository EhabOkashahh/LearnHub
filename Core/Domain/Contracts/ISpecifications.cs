using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Contracts
{
    public interface ISpecifications<TKey,TEntity> where TEntity : BaseEntity<TKey>
    {
        public Expression<Func<TEntity, bool>>? Criteria  { get; set; }

        public List<Func<IQueryable<TEntity>,IQueryable<TEntity>>> IncludeAction {get; set;}

        public Expression<Func<TEntity, object>>? OrderByAsc { get; set; }
        public Expression<Func<TEntity, object>>? OrderByDesc { get; set; }

        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginated { get; set; }

        void AddInclude(Func<IQueryable<TEntity>,IQueryable<TEntity>> action);
    }
}