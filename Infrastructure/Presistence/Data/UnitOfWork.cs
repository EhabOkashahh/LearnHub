using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using Presistence.Data.Contexts;
using Presistence.Repository;

namespace Presistence.Data
{
    public class UnitOfWork(AppDbContext _context) : IUnitOfWork
    {
        private ConcurrentDictionary<Type, object> repositories = new ConcurrentDictionary<Type, object>();
        public IGenericRepository<Tkey, TEntity> GetRepository<Tkey, TEntity>() where TEntity : BaseEntity<Tkey>
        {
            
           return (GenericRepository<Tkey,TEntity>) repositories.GetOrAdd(typeof(TEntity), new GenericRepository<Tkey, TEntity>(_context));
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return await _context.SaveChangesAsync(ct);
        }
    }
}