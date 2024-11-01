using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GHSTShipping.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusForOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentStatus",
                table: "Order",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentStatus",
                table: "Order");
        }
    }
}
