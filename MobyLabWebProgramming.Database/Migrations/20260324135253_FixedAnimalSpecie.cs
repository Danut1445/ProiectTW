using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobyLabWebProgramming.Database.Migrations
{
    /// <inheritdoc />
    public partial class FixedAnimalSpecie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AnimalSpecies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AnimalSpecies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AnimalSpecies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AnimalSpecies_Id",
                table: "AnimalSpecies",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AnimalSpecies_Id",
                table: "AnimalSpecies");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AnimalSpecies");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AnimalSpecies");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AnimalSpecies");
        }
    }
}
