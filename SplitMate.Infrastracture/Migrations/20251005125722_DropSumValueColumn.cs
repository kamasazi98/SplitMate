using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SplitMate.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class DropSumValueColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SumValue",
                table: "ShoppingLists");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SumValue",
                table: "ShoppingLists",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
