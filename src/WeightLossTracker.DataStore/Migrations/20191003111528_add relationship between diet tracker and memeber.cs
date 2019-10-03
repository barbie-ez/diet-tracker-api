using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeightLossTracker.DataStore.Migrations
{
    public partial class addrelationshipbetweendiettrackerandmemeber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    DietaryFiber = table.Column<float>(nullable: false),
                    Carbohydrates = table.Column<float>(nullable: false),
                    Fats = table.Column<float>(nullable: false),
                    Protein = table.Column<float>(nullable: false),
                    Sugars = table.Column<float>(nullable: false),
                    Calories = table.Column<int>(nullable: false),
                    ServingSize = table.Column<string>(nullable: false),
                    FoodName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealCategoriesModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealCategoriesModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DietTrackerModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    MealCategoriesId = table.Column<int>(nullable: false),
                    MemberId = table.Column<string>(nullable: false),
                    FoodModelId = table.Column<int>(nullable: false),
                    PortionSize = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietTrackerModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DietTrackerModel_FoodModel_FoodModelId",
                        column: x => x.FoodModelId,
                        principalTable: "FoodModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DietTrackerModel_MealCategoriesModel_MealCategoriesId",
                        column: x => x.MealCategoriesId,
                        principalTable: "MealCategoriesModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DietTrackerModel_AspNetUsers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DietTrackerModel_FoodModelId",
                table: "DietTrackerModel",
                column: "FoodModelId");

            migrationBuilder.CreateIndex(
                name: "IX_DietTrackerModel_MealCategoriesId",
                table: "DietTrackerModel",
                column: "MealCategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_DietTrackerModel_MemberId",
                table: "DietTrackerModel",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DietTrackerModel");

            migrationBuilder.DropTable(
                name: "FoodModel");

            migrationBuilder.DropTable(
                name: "MealCategoriesModel");
        }
    }
}
