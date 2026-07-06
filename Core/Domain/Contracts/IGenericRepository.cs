using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Contracts
{
    public interface IGenericRepository<Tkey, TEntity> where TEntity : BaseEntity<Tkey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<Tkey,TEntity> spec , CancellationToken ct, bool ChangeTrackr = false);
        Task<TEntity?> GetAsync(ISpecifications<Tkey,TEntity> spec, Tkey key, CancellationToken ct);
        Task AddAsync(TEntity entity);
        Task<int> GetCountAsync(ISpecifications<Tkey,TEntity> spec);
        void Update(TEntity entity);
        void Delete(Tkey key);
    }
}