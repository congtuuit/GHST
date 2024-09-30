using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GHSTShipping.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PartnerConfigActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartnerName",
                table: "PartnerConfig");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryPartner",
                table: "PartnerConfig",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActivated",
                table: "PartnerConfig",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryPartner",
                table: "PartnerConfig");

            migrationBuilder.DropColumn(
                name: "IsActivated",
                table: "PartnerConfig");

            migrationBuilder.AddColumn<string>(
                name: "PartnerName",
                table: "PartnerConfig",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
