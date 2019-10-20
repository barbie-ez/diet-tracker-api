using Microsoft.EntityFrameworkCore.Migrations;

namespace WeightLossTracker.DataStore.Migrations
{
    public partial class newrelationshipkeyforweighthistoriesdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeightHistories_AspNetUsers_UserProfileModelId",
                table: "WeightHistories");

            migrationBuilder.RenameColumn(
                name: "UserProfileModelId",
                table: "WeightHistories",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_WeightHistories_UserProfileModelId",
                table: "WeightHistories",
                newName: "IX_WeightHistories_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeightHistories_AspNetUsers_UserId",
                table: "WeightHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeightHistories_AspNetUsers_UserId",
                table: "WeightHistories");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "WeightHistories",
                newName: "UserProfileModelId");

            migrationBuilder.RenameIndex(
                name: "IX_WeightHistories_UserId",
                table: "WeightHistories",
                newName: "IX_WeightHistories_UserProfileModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeightHistories_AspNetUsers_UserProfileModelId",
                table: "WeightHistories",
                column: "UserProfileModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
