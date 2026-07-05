using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Presistence.Data.Contexts;

namespace Presistence
{
    public static class SpecificationsEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TKey,TEntity>(IQueryable<TEntity> InputQuery, ISpecifications<TKey , TEntity> spec) where TEntity : BaseEntity<TKey>
        {
            var query = InputQuery;

            if(spec.IncludeExpression.Count() > 0)
            {
                spec.IncludeExpression.Aggregate(query, (current, includeExpession) => current.Include(includeExpession));
            }

            if(spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }

            return query;
        }
    }
}