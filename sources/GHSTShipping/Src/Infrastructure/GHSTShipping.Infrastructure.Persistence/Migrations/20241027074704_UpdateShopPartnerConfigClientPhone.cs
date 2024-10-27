using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GHSTShipping.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShopPartnerConfigClientPhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientPhone",
                table: "ShopPartnerConfig",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientPhone",
                table: "ShopPartnerConfig");
        }
    }
}
