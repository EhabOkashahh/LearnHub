using Domain.Entities.Courses;
using Microsoft.EntityFrameworkCore;

namespace Domain.Contracts
{
    public interface IAppDbContext
    {
        IQueryable<Course> Courses { get; }
        IQueryable<CourseSection> CourseSections { get; }
        IQueryable<Lesson> Lessons { get; }
        IQueryable<Category> Categories { get; }
        Task<int> SaveChangesAsync(CancellationToken ct);
        void Add<TEntity>(TEntity entity) where TEntity : class;
        void Remove<TEntity>(TEntity entity) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;        
    }
}