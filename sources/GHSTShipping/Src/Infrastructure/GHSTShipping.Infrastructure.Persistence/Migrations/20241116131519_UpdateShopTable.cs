using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GHSTShipping.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShopTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Shop",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "Shop",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistrictName",
                table: "Shop",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Shop",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProvinceId",
                table: "Shop",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvinceName",
                table: "Shop",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WardId",
                table: "Shop",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WardName",
                table: "Shop",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Shop");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Shop");

            migrationBuilder.DropColumn(
                name: "DistrictName",
                table: "Shop");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Shop");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "Shop");

            migrationBuilder.DropColumn(
                name: "ProvinceName",
                table: "Shop");

            migrationBuilder.DropColumn(
                name: "WardId",
                table: "Shop");

            migrationBuilder.DropColumn(
                name: "WardName",
                table: "Shop");
        }
    }
}
