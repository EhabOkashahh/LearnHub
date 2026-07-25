using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Courses;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Presistence.Interceptors
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if(context is null) return ValueTask.FromResult(result);
            var entries = context.ChangeTracker.Entries<AppUser>();

            var deletedUserIds = entries.Where(e => e.State == EntityState.Modified && 
                                                  e.Property(x => x.IsDeleted).IsModified && e.Entity.IsDeleted)
                                            .Select(e => e.Entity.Id)
                                            .ToList();

            if(deletedUserIds.Count() == 0) return ValueTask.FromResult(result);

            var enrollments = context.Set<Enrollment>().Where(e => deletedUserIds.Contains(e.StudentId) && !e.IsDeleted).ToList();
             foreach (var e in enrollments)
            {
                e.IsDeleted = true;
                e.DeletedAt = DateTime.UtcNow;
            }

            var studentProgress = context.Set<LessonProgress>()
                .Where(p => deletedUserIds.Contains(p.StudentId) && !p.IsDeleted)
                .ToList();
            foreach (var p in studentProgress)
            {
                p.IsDeleted = true;
                p.DeletedAt = DateTime.UtcNow;
            }

            var requests = context.Set<InstructorRequest>()
                .Where(r => deletedUserIds.Contains(r.UserId) && !r.IsDeleted)
                .ToList();
            foreach (var r in requests)
            {
                r.IsDeleted = true;
                r.DeletedAt = DateTime.UtcNow;
            }

            var courses = context.Set<Course>()
                .Where(c => deletedUserIds.Contains(c.InstructorId) && !c.IsDeleted)
                .ToList();
            foreach (var c in courses)
            {
                c.IsDeleted = true;
                c.DeletedAt = DateTime.UtcNow;
            }

            var deletedCourseIds = courses.Select(c => c.Id).ToList();

            var sections = context.Set<CourseSection>()
                .Where(s => deletedCourseIds.Contains(s.CourseId) && !s.IsDeleted)
                .ToList();
            foreach (var s in sections)
            {
                s.IsDeleted = true;
                s.DeletedAt = DateTime.UtcNow;
            }

            var deletedSectionIds = sections.Select(s => s.Id).ToList();

            var lessons = context.Set<Lesson>()
                .Where(l => deletedSectionIds.Contains(l.SectionId) && !l.IsDeleted)
                .ToList();
            foreach (var l in lessons)
            {
                l.IsDeleted = true;
                l.DeletedAt = DateTime.UtcNow;
            }

            var deletedLessonIds = lessons.Select(l => l.Id).ToList();

            var instructorProgress = context.Set<LessonProgress>()
                .Where(p => deletedLessonIds.Contains(p.LessonId) && !p.IsDeleted)
                .ToList();
            foreach (var p in instructorProgress)
            {
                p.IsDeleted = true;
                p.DeletedAt = DateTime.UtcNow;
            }
            
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}