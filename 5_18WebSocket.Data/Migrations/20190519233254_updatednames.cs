using Microsoft.EntityFrameworkCore.Migrations;

namespace _5_18WebSocket.Data.Migrations
{
    public partial class updatednames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_UserAssignedId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_UserAssignedId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "UserAssignedId",
                table: "Tasks");

            migrationBuilder.AddColumn<string>(
                name: "UserNameAssigned",
                table: "Tasks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserNameAssigned",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "UserAssignedId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserAssignedId",
                table: "Tasks",
                column: "UserAssignedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_UserAssignedId",
                table: "Tasks",
                column: "UserAssignedId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
