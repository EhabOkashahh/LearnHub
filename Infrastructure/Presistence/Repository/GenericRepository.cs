using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Presistence.Data.Contexts;

namespace Presistence.Repository
{
    public class GenericRepository<Tkey, TEntity>(AppDbContext _context) : IGenericRepository<Tkey, TEntity> where TEntity : BaseEntity<Tkey>
    {
        private IQueryable<TEntity> InputQuery { get; set; } = _context.Set<TEntity>();
        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<Tkey,TEntity> spec, CancellationToken ct ,bool ChangeTrackr = false)
        {
           return ChangeTrackr ? await SpecificationsEvaluator.GetQuery(InputQuery, spec).ToListAsync(ct) :
                                  await SpecificationsEvaluator.GetQuery(InputQuery, spec).AsNoTracking().ToListAsync(ct);
        }

        public async Task<TEntity?> GetAsync(ISpecifications<Tkey,TEntity> spec, Tkey key, CancellationToken ct)
        {
            return await SpecificationsEvaluator.GetQuery(InputQuery, spec).FirstOrDefaultAsync(ct);
        }

        public async Task AddAsync(TEntity entity)
        {
           await _context.AddAsync(entity);
        }

        public async Task<int> GetCountAsync(ISpecifications<Tkey, TEntity> spec)
        {
            return await SpecificationsEvaluator.GetQuery(InputQuery, spec).CountAsync();
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
            }
        }

        public async Task<bool> IsExsists(ISpecifications<Tkey, TEntity> spec)
        {
            return await SpecificationsEvaluator.GetQuery(InputQuery, spec).AnyAsync();    
        }
    }
}