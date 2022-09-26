using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialMediaMustBeGood2.Migrations
{
    public partial class NewPublicationsViewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<int>>(
                name: "CommentsUsersId",
                table: "Publications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentsUsersId",
                table: "Publications");
        }
    }
}
