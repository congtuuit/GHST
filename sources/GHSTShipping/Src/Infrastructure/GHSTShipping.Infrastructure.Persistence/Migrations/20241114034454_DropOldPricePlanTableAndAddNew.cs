using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GHSTShipping.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DropOldPricePlanTableAndAddNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopPricePlan");

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryPricePlaneId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeliveryPricePlane",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedToDeliveryPricePlaneId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MinWeight = table.Column<long>(type: "bigint", nullable: false),
                    MaxWeight = table.Column<long>(type: "bigint", nullable: false),
                    PublicPrice = table.Column<long>(type: "bigint", nullable: false),
                    PrivatePrice = table.Column<long>(type: "bigint", nullable: false),
                    StepPrice = table.Column<long>(type: "bigint", nullable: false),
                    StepWeight = table.Column<long>(type: "bigint", nullable: false),
                    LimitInsurance = table.Column<long>(type: "bigint", nullable: false),
                    InsuranceFeeRate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ReturnFeeRate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ConvertWeightRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPricePlane", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryPricePlane");

            migrationBuilder.DropColumn(
                name: "DeliveryPricePlaneId",
                table: "Order");

            migrationBuilder.CreateTable(
                name: "ShopPricePlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConvertRate = table.Column<int>(type: "int", nullable: false),
                    ConvertedWeight = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Height = table.Column<int>(type: "int", precision: 18, scale: 2, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Length = table.Column<int>(type: "int", precision: 18, scale: 2, nullable: false),
                    OfficialPrice = table.Column<long>(type: "bigint", precision: 18, scale: 2, nullable: false),
                    PrivatePrice = table.Column<long>(type: "bigint", precision: 18, scale: 2, nullable: false),
                    Supplier = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Weight = table.Column<int>(type: "int", precision: 18, scale: 2, nullable: false),
                    Width = table.Column<int>(type: "int", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopPricePlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopPricePlan_Shop_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopPricePlan_ShopId",
                table: "ShopPricePlan",
                column: "ShopId");
        }
    }
}
