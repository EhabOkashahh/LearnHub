using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Presistence.Data.Contexts;

namespace Presistence.Repository
{
    public class GenericRepository<Tkey, TEntity>(AppDbContext _context) : IGenericRepository<Tkey, TEntity> where TEntity : BaseEntity<Tkey>
    {
        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<Tkey,TEntity> spec, CancellationToken ct ,bool ChangeTrackr = false)
        {
           return await SpecificationsEvaluator.GetQuery(_context.Set<TEntity>(), spec).ToListAsync(ct);
        }
        public async Task<TEntity> GetAsync(ISpecifications<Tkey,TEntity> spec, Tkey key, CancellationToken ct)
        {
            return await SpecificationsEvaluator.GetQuery(_context.Set<TEntity>(), spec).FirstOrDefaultAsync(ct);
        }
        public async Task AddAsync(TEntity entity)
        {
           await _context.AddAsync(entity);
        }
        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }
        public void Delete(Tkey key)
        {
            var entity = _context.Set<TEntity>().Find(key);
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.UtcNow;
                _context.Update(entity);
            }
        }
    }
}