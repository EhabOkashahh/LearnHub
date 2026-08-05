using Domain.Entities.Enums;
using Domain.Entities.Identity;
using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistence.Data.Configurations.OrderConfigrations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.OrderNumber)
                .IsRequired()
                .HasMaxLength(32);
            builder.HasIndex(x => x.OrderNumber)
                .IsUnique();

            builder.Property(x => x.StudentId)
                .IsRequired();

            builder.Property(x => x.Subtotal)
                .HasColumnType("decimal(18,2)");
            builder.Property(x => x.DiscountAmount)
                .HasColumnType("decimal(18,2)");
            builder.Property(x => x.Total)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(OrderStatus.pending);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasQueryFilter(x => !x.IsDeleted);

            builder.HasOne(x => x.Student)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Items)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
