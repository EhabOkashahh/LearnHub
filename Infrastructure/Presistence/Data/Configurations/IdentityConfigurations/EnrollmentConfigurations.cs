using Domain.Entities.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistence.Data.Configurations.IdentityConfigurations
{
    public class EnrollmentConfigurations : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(x => new { x.StudentProfileId, x.CourseId });

            builder.Property(x => x.ProgressPercentage)
                .HasDefaultValue(0.0);

            builder.Property(x => x.EnrolledAt)
                .IsRequired();

            builder.HasOne(x => x.StudentProfile)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.StudentProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Course)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
