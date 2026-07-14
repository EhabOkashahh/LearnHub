using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistence.Data.Configurations.IdentityConfigurations
{
    public class AppUserConfigurations : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.DisplayName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.HeadLine)
                .HasMaxLength(200);

            builder.Property(x => x.Bio)
                .HasMaxLength(1000);

            builder.Property(x => x.ProfilePictureUrl)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne(x => x.StudentProfile)
                .WithOne(x => x.AppUser)
                .HasForeignKey<StudentProfile>(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.InstructorProfile)
                .WithOne(x => x.AppUser)
                .HasForeignKey<InstructorProfile>(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
