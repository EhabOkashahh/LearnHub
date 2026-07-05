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
        public List<Expression<Func<TEntity, object>>> IncludeExpression { get; set; }
        public Expression<Func<TEntity, bool>>? Criteria  { get; set; }
    }
}