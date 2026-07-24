using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistence.Data.Configurations.IdentityConfigurations
{
    public class InstructorRequestConfigurations : IEntityTypeConfiguration<InstructorRequest>
    {
        public void Configure(EntityTypeBuilder<InstructorRequest> builder)
        {
            builder.HasKey(x => x.Id);

             builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}