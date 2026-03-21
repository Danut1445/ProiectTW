using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MobyLabWebProgramming.Database.Repository.Entities;
using MobyLabWebProgramming.Database.Repository.Enums;

namespace MobyLabWebProgramming.Database.Repository.EntityConfigurations;

public class AnimalSpeciesConfiguration : IEntityTypeConfiguration<AnimalSpecies>
{
    public void Configure(EntityTypeBuilder<AnimalSpecies> builder)
    {
        builder.Property(e => e.Specie)
            .HasMaxLength(255)
            .IsRequired();
        builder.HasKey(e => e.Specie);
        builder.Property(e => e.Specie)
            .HasMaxLength(4096)
            .IsRequired();
        builder.Property(e => e.FoodType)
            .HasConversion(new EnumToStringConverter<FoodTypesEnum>())
            .HasMaxLength(255)
            .IsRequired();
    }
}