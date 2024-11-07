using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GHSTShipping.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreColumnShopDeliveryConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "ShopPartnerConfig",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistrictName",
                table: "ShopPartnerConfig",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProviceName",
                table: "ShopPartnerConfig",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WardName",
                table: "ShopPartnerConfig",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "ShopPartnerConfig");

            migrationBuilder.DropColumn(
                name: "DistrictName",
                table: "ShopPartnerConfig");

            migrationBuilder.DropColumn(
                name: "ProviceName",
                table: "ShopPartnerConfig");

            migrationBuilder.DropColumn(
                name: "WardName",
                table: "ShopPartnerConfig");
        }
    }
}
