using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GHSTShipping.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ClonePartnerShopInfoToDeliveryPricePlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "DeliveryPricePlane",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientPhone",
                table: "DeliveryPricePlane",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistrictId",
                table: "DeliveryPricePlane",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistrictName",
                table: "DeliveryPricePlane",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PartnerConfigId",
                table: "DeliveryPricePlane",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "PartnerShopId",
                table: "DeliveryPricePlane",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvinceId",
                table: "DeliveryPricePlane",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvinceName",
                table: "DeliveryPricePlane",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShopName",
                table: "DeliveryPricePlane",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WardCode",
                table: "DeliveryPricePlane",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WardName",
                table: "DeliveryPricePlane",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "DeliveryPricePlane");

            migrationBuilder.DropColumn(
                name: "ClientPhone",
                table: "DeliveryPricePlane");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "DeliveryPricePlane");

            migrationBuilder.DropColumn(
                name: "DistrictName",
                table: "DeliveryPricePlane");

            migrationBuilder.DropColumn(
                name: "PartnerConfigId",
                table: "DeliveryPricePlane");

            migrationBuilder.DropColumn(
                name: "PartnerShopId",
                table: "DeliveryPricePlane");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "DeliveryPricePlane");

            migrationBuilder.DropColumn(
                name: "ProvinceName",
                table: "DeliveryPricePlane");

            migrationBuilder.DropColumn(
                name: "ShopName",
                table: "DeliveryPricePlane");

            migrationBuilder.DropColumn(
                name: "WardCode",
                table: "DeliveryPricePlane");

            migrationBuilder.DropColumn(
                name: "WardName",
                table: "DeliveryPricePlane");
        }
    }
}
