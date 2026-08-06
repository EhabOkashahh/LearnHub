using Domain.Entities.Courses;
using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistence.Data.Configurations.OrderConfigrations
{
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItems>
    {
        public void Configure(EntityTypeBuilder<OrderItems> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.OrderId)
                .IsRequired();

            builder.Property(x => x.CourseId)
                .IsRequired();

            builder.Property(x => x.CourseTitle)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.ThumbnailUrl)
                .HasMaxLength(500);

            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.OriginalPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasQueryFilter(x => !x.IsDeleted);

            builder.HasOne(x => x.Order)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
