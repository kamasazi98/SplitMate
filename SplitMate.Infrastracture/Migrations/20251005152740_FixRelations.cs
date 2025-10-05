using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SplitMate.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItems_KnownUsers_UserId",
                table: "ShoppingItems");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ShoppingItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItems_KnownUsers_UserId",
                table: "ShoppingItems",
                column: "UserId",
                principalTable: "KnownUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingItems_KnownUsers_UserId",
                table: "ShoppingItems");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ShoppingItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingItems_KnownUsers_UserId",
                table: "ShoppingItems",
                column: "UserId",
                principalTable: "KnownUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
