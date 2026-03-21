using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MobyLabWebProgramming.Database.Repository.Entities;
using MobyLabWebProgramming.Database.Repository.Enums;

namespace MobyLabWebProgramming.Database.Repository.EntityConfigurations;

public class FoodStockpileConfiguration : IEntityTypeConfiguration<FoodStockpile>
{
    public void Configure(EntityTypeBuilder<FoodStockpile> builder)
    {
        // Food columns configuration:
        builder.Property(e => e.Grains) // This specifies which property is configured.
            .IsRequired(); // Here it is specified if the property is required, meaning it cannot be null in the database.
        builder.Property(e => e.Meats) // This specifies which property is configured.
            .IsRequired(); // Here it is specified if the property is required, meaning it cannot be null in the database.
        builder.Property(e => e.FishFood) // This specifies which property is configured.
            .IsRequired(); // Here it is specified if the property is required, meaning it cannot be null in the database.
        builder.Property(e => e.Plants) // This specifies which property is configured.
            .IsRequired(); // Here it is specified if the property is required, meaning it cannot be null in the database.
        builder.Property(e => e.Fish) // This specifies which property is configured.
            .IsRequired(); // Here it is specified if the property is required, meaning it cannot be null in the database.
        builder.Property(e => e.UserId)
            .IsRequired();
        builder.HasKey(e => e.UserId);
        
        // One-to-one configuration with User:
        builder.HasOne(e => e.User)
            .WithOne(e => e.FoodStockpile)
            .HasForeignKey<FoodStockpile>(e => e.UserId)
            .HasPrincipalKey<User>(e => e.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}