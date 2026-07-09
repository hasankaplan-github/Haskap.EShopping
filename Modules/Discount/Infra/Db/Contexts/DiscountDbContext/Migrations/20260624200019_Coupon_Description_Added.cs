using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Discount.Infra.Db.Contexts.DiscountDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Coupon_Description_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                schema: "discount",
                table: "special_coupon",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                schema: "discount",
                table: "regular_coupon",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                schema: "discount",
                table: "special_coupon");

            migrationBuilder.DropColumn(
                name: "description",
                schema: "discount",
                table: "regular_coupon");
        }
    }
}
