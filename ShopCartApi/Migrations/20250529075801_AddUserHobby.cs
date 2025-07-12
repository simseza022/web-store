using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopCartApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserHobby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "UserHobby",
                schema: "ShopCartAppSchema",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserHobby",
                schema: "ShopCartAppSchema",
                table: "Posts");
        }
    }
}
