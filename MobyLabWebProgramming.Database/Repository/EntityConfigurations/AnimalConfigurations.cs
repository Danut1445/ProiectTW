using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MobyLabWebProgramming.Database.Repository.Entities;
using MobyLabWebProgramming.Database.Repository.Enums;

namespace MobyLabWebProgramming.Database.Repository.EntityConfigurations;

public class AnimalConfigurations : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.Property(e => e.Id) // This specifies which property is configured.
            .IsRequired(); // Here it is specified if the property is required, meaning it cannot be null in the database.
        builder.HasKey(x => x.Id); // Here it is specified that the property Id is the primary key.
        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired();
        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(e => e.Happiness)
            .IsRequired();
        builder.Property(e => e.Happiness)
            .IsRequired();
        
        // Configuring One-to-Many relationship with User
        builder.Property(e => e.Id)
            .IsRequired();
        builder.HasOne(e => e.User)
            .WithMany(e => e.Animals)
            .HasForeignKey(e => e.UserId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Configuring One-to-Many relationship with Specie
        builder.Property(e => e.BornSpecie)
            .HasMaxLength(255)
            .IsRequired();
        builder.HasOne(e => e.AnimalSpecie)
            .WithMany(e => e.Animals)
            .HasForeignKey(e => e.BornSpecie)
            .HasPrincipalKey(e => e.Specie)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
