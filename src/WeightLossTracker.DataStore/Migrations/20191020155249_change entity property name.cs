using Microsoft.EntityFrameworkCore.Migrations;

namespace WeightLossTracker.DataStore.Migrations
{
    public partial class changeentitypropertyname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DietTrackerModel_FoodModel_FoodModelId",
                table: "DietTrackerModel");

            migrationBuilder.RenameColumn(
                name: "FoodModelId",
                table: "DietTrackerModel",
                newName: "FoodId");

            migrationBuilder.RenameIndex(
                name: "IX_DietTrackerModel_FoodModelId",
                table: "DietTrackerModel",
                newName: "IX_DietTrackerModel_FoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_DietTrackerModel_FoodModel_FoodId",
                table: "DietTrackerModel",
                column: "FoodId",
                principalTable: "FoodModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DietTrackerModel_FoodModel_FoodId",
                table: "DietTrackerModel");

            migrationBuilder.RenameColumn(
                name: "FoodId",
                table: "DietTrackerModel",
                newName: "FoodModelId");

            migrationBuilder.RenameIndex(
                name: "IX_DietTrackerModel_FoodId",
                table: "DietTrackerModel",
                newName: "IX_DietTrackerModel_FoodModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_DietTrackerModel_FoodModel_FoodModelId",
                table: "DietTrackerModel",
                column: "FoodModelId",
                principalTable: "FoodModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
