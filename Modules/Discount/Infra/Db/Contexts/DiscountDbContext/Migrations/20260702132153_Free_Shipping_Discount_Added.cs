using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Discount.Infra.Db.Contexts.DiscountDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Free_Shipping_Discount_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "discount_is_free_shipping_discount",
                schema: "discount",
                table: "special_coupon",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "discount_is_free_shipping_discount",
                schema: "discount",
                table: "regular_coupon",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discount_is_free_shipping_discount",
                schema: "discount",
                table: "special_coupon");

            migrationBuilder.DropColumn(
                name: "discount_is_free_shipping_discount",
                schema: "discount",
                table: "regular_coupon");
        }
    }
}
