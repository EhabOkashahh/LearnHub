using Domain.Entities.Courses;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistence.Data.Configurations.CourseConfigrations
{
    public class LessonProgressConfigurations : IEntityTypeConfiguration<LessonProgress>
    {
        public void Configure(EntityTypeBuilder<LessonProgress> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.StudentId, x.LessonId }).IsUnique();

            builder.HasQueryFilter(x => !x.IsDeleted);

            builder.Property(x => x.IsCompleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Lesson)
                .WithMany()
                .HasForeignKey(x => x.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}