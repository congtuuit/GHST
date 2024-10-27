using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GHSTShipping.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpShopCOnfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProviceName",
                table: "ShopPartnerConfig",
                newName: "WardCode");

            migrationBuilder.AddColumn<string>(
                name: "DistrictId",
                table: "ShopPartnerConfig",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvinceId",
                table: "ShopPartnerConfig",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvinceName",
                table: "ShopPartnerConfig",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "ShopPartnerConfig");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "ShopPartnerConfig");

            migrationBuilder.DropColumn(
                name: "ProvinceName",
                table: "ShopPartnerConfig");

            migrationBuilder.RenameColumn(
                name: "WardCode",
                table: "ShopPartnerConfig",
                newName: "ProviceName");
        }
    }
}
