using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Discount.Infra.Db.Contexts.DiscountDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "discount");

            migrationBuilder.CreateTable(
                name: "regular_coupon",
                schema: "discount",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    basket_min_total_amount_value = table.Column<decimal>(type: "numeric", nullable: false),
                    basket_min_total_amount_currency = table.Column<string>(type: "text", nullable: false),
                    usage_count_value = table.Column<int>(type: "integer", nullable: false),
                    usage_count_limit = table.Column<int>(type: "integer", nullable: false),
                    discount_quantity_divider = table.Column<int>(type: "integer", nullable: true),
                    discount_fixed_price_value = table.Column<decimal>(type: "numeric", nullable: true),
                    discount_fixed_discount_amount_value = table.Column<decimal>(type: "numeric", nullable: true),
                    discount_percentage_discount_value = table.Column<decimal>(type: "numeric", nullable: true),
                    date_range_utc_start_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_range_utc_end_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_regular_coupon", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "special_coupon",
                schema: "discount",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    basket_min_total_amount_value = table.Column<decimal>(type: "numeric", nullable: false),
                    basket_min_total_amount_currency = table.Column<string>(type: "text", nullable: false),
                    usage_count_value = table.Column<int>(type: "integer", nullable: false),
                    usage_count_limit = table.Column<int>(type: "integer", nullable: false),
                    discount_quantity_divider = table.Column<int>(type: "integer", nullable: true),
                    discount_fixed_price_value = table.Column<decimal>(type: "numeric", nullable: true),
                    discount_fixed_discount_amount_value = table.Column<decimal>(type: "numeric", nullable: true),
                    discount_percentage_discount_value = table.Column<decimal>(type: "numeric", nullable: true),
                    date_range_utc_start_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_range_utc_end_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_special_coupon", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "coupon_category",
                schema: "discount",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    coupon_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coupon_category", x => x.id);
                    table.ForeignKey(
                        name: "fk_coupon_category_regular_coupon_coupon_id",
                        column: x => x.coupon_id,
                        principalSchema: "discount",
                        principalTable: "regular_coupon",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_coupon_category_special_coupon_coupon_id",
                        column: x => x.coupon_id,
                        principalSchema: "discount",
                        principalTable: "special_coupon",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "coupon_selected_product_variant",
                schema: "discount",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    coupon_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coupon_selected_product_variant", x => x.id);
                    table.ForeignKey(
                        name: "fk_coupon_selected_product_variant_regular_coupon_coupon_id",
                        column: x => x.coupon_id,
                        principalSchema: "discount",
                        principalTable: "regular_coupon",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_coupon_selected_product_variant_special_coupon_coupon_id",
                        column: x => x.coupon_id,
                        principalSchema: "discount",
                        principalTable: "special_coupon",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_coupon_category_coupon_id",
                schema: "discount",
                table: "coupon_category",
                column: "coupon_id");

            migrationBuilder.CreateIndex(
                name: "ix_coupon_selected_product_variant_coupon_id",
                schema: "discount",
                table: "coupon_selected_product_variant",
                column: "coupon_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "coupon_category",
                schema: "discount");

            migrationBuilder.DropTable(
                name: "coupon_selected_product_variant",
                schema: "discount");

            migrationBuilder.DropTable(
                name: "regular_coupon",
                schema: "discount");

            migrationBuilder.DropTable(
                name: "special_coupon",
                schema: "discount");
        }
    }
}
