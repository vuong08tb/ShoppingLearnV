using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingLearn.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountToCoupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Coupons",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0.5m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Coupons");
        }
    }
}
