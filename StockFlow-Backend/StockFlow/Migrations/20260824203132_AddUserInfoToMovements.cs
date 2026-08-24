using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockFlow.Migrations
{
    /// <inheritdoc />
    public partial class AddUserInfoToMovements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Movements",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Movements",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Movements");
        }
    }
}
