using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobyLabWebProgramming.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedFoodStockpile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodStockpile",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Grains = table.Column<int>(type: "integer", nullable: false),
                    Meats = table.Column<int>(type: "integer", nullable: false),
                    FishFood = table.Column<int>(type: "integer", nullable: false),
                    Plants = table.Column<int>(type: "integer", nullable: false),
                    Fish = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodStockpile", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_FoodStockpile_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodStockpile");
        }
    }
}
