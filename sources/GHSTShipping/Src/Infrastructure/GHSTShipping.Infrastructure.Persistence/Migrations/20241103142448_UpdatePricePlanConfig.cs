using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GHSTShipping.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePricePlanConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "ShopPricePlan",
                newName: "Width");

            migrationBuilder.AddColumn<decimal>(
                name: "ConvertedWeight",
                table: "ShopPricePlan",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Height",
                table: "ShopPricePlan",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Length",
                table: "ShopPricePlan",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "ShopPricePlan",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConvertedWeight",
                table: "ShopPricePlan");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "ShopPricePlan");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "ShopPricePlan");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "ShopPricePlan");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "ShopPricePlan",
                newName: "Capacity");
        }
    }
}
