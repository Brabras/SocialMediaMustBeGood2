using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialMediaMustBeGood2.Migrations
{
    public partial class AuthorIdToUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publications_AspNetUsers_AuthorId",
                table: "Publications");

            migrationBuilder.DropIndex(
                name: "IX_Publications_AuthorId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Publications");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Publications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Publications_UserId",
                table: "Publications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_AspNetUsers_UserId",
                table: "Publications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publications_AspNetUsers_UserId",
                table: "Publications");

            migrationBuilder.DropIndex(
                name: "IX_Publications_UserId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Publications");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Publications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Publications_AuthorId",
                table: "Publications",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_AspNetUsers_AuthorId",
                table: "Publications",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
