using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GHSTShipping.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderTableFeeCallBack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "private_expected_delivery_time",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "private_operation_partner",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "private_order_code",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "private_sort_code",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "private_total_fee",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "private_trans_type",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "private_expected_delivery_time",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "private_operation_partner",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "private_order_code",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "private_sort_code",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "private_total_fee",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "private_trans_type",
                table: "Order");
        }
    }
}
