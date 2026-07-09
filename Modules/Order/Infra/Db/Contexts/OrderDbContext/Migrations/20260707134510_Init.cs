using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Order.Infra.Db.Contexts.OrderDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "order");

            migrationBuilder.CreateTable(
                name: "order",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code_value = table.Column<string>(type: "text", nullable: false),
                    account_owner_account_id = table.Column<Guid>(type: "uuid", nullable: true),
                    account_first_name = table.Column<string>(type: "text", nullable: false),
                    account_last_name = table.Column<string>(type: "text", nullable: false),
                    account_phone_number = table.Column<string>(type: "text", nullable: false),
                    account_email_address = table.Column<string>(type: "text", nullable: false),
                    shipping_address_owner_shipping_address_id = table.Column<Guid>(type: "uuid", nullable: false),
                    shipping_address_city_name = table.Column<string>(type: "text", nullable: false),
                    shipping_address_district_name = table.Column<string>(type: "text", nullable: false),
                    shipping_address_neighborhood_name = table.Column<string>(type: "text", nullable: false),
                    shipping_address_street = table.Column<string>(type: "text", nullable: false),
                    shipping_address_postcode = table.Column<string>(type: "text", nullable: true),
                    shipping_address_building_no = table.Column<string>(type: "text", nullable: false),
                    shipping_address_floor = table.Column<int>(type: "integer", nullable: true),
                    shipping_address_apartment_no = table.Column<int>(type: "integer", nullable: true),
                    shipping_address_address_line = table.Column<string>(type: "text", nullable: true),
                    has_free_shipping_coupon = table.Column<bool>(type: "boolean", nullable: false),
                    shipping_fee_value = table.Column<decimal>(type: "numeric", nullable: false),
                    shipping_fee_currency = table.Column<string>(type: "text", nullable: false),
                    total_value = table.Column<decimal>(type: "numeric", nullable: false),
                    total_currency = table.Column<string>(type: "text", nullable: false),
                    total_with_discount_value = table.Column<decimal>(type: "numeric", nullable: false),
                    total_with_discount_currency = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "applied_coupon",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_coupon_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    discount_amount_value = table.Column<decimal>(type: "numeric", nullable: false),
                    discount_amount_currency = table.Column<string>(type: "text", nullable: false),
                    is_special_coupon = table.Column<bool>(type: "boolean", nullable: false),
                    code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_applied_coupon", x => x.id);
                    table.ForeignKey(
                        name: "fk_applied_coupon_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "order",
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_name = table.Column<string>(type: "text", nullable: false),
                    variant_sku_value = table.Column<string>(type: "text", nullable: false),
                    slug_value = table.Column<string>(type: "text", nullable: false),
                    price_value = table.Column<decimal>(type: "numeric", nullable: false),
                    price_currency = table.Column<string>(type: "text", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    total_value = table.Column<decimal>(type: "numeric", nullable: false),
                    total_currency = table.Column<string>(type: "text", nullable: false),
                    total_with_discount_value = table.Column<decimal>(type: "numeric", nullable: false),
                    total_with_discount_currency = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_item_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "order",
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_applied_coupon_order_id",
                schema: "order",
                table: "applied_coupon",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_item_order_id",
                schema: "order",
                table: "item",
                column: "order_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "applied_coupon",
                schema: "order");

            migrationBuilder.DropTable(
                name: "item",
                schema: "order");

            migrationBuilder.DropTable(
                name: "order",
                schema: "order");
        }
    }
}
