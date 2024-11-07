using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GHSTShipping.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomOrderSizeToOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RootHeight",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RootLength",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RootWeight",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RootWidth",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RootHeight",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "RootLength",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "RootWeight",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "RootWidth",
                table: "Order");
        }
    }
}
