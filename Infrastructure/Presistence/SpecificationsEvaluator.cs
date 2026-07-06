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

            // Filtering
            if(spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }

            
            


            // Sorting
            if(spec.OrderByAsc is not null)
            {
                query = query.OrderBy(spec.OrderByAsc);
            }
            else if(spec.OrderByDesc is not null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            // Pagination
            if (spec.IsPaginated)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }



            if(spec.IncludeExpression.Count > 0)
            {
                query = spec.IncludeExpression.Aggregate(query, (current, includeExpession) => current.Include(includeExpession));
            }

            return query;
        }
    }
}