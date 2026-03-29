using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MobyLabWebProgramming.Database.Repository.Entities;
using MobyLabWebProgramming.Database.Repository.Enums;

namespace MobyLabWebProgramming.Database.Repository.EntityConfigurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(e => e.OrderId)
            .IsRequired();
        builder.Property(e => e.Id).IsRequired();
        builder.HasAlternateKey(e => e.Id);
        builder.Property(e => e.ItemId)
            .IsRequired();
        builder.HasKey(e => new { e.OrderId, e.ItemId });
        builder.Property(e => e.Quantity)
            .IsRequired();
        
        builder.HasOne(e => e.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(e => e.OrderId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(e => e.ShopItem)
            .WithMany(s => s.OrderItems)
            .HasForeignKey(e => e.ItemId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}