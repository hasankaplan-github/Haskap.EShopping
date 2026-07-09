using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Discount.Infra.Db.Contexts.DiscountDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Coupon_Edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "coupon_category",
                schema: "discount");

            migrationBuilder.DropTable(
                name: "coupon_selected_product_variant",
                schema: "discount");

            migrationBuilder.CreateTable(
                name: "regular_coupon_category",
                schema: "discount",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    regular_coupon_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_regular_coupon_category", x => x.id);
                    table.ForeignKey(
                        name: "fk_regular_coupon_category_regular_coupon_regular_coupon_id",
                        column: x => x.regular_coupon_id,
                        principalSchema: "discount",
                        principalTable: "regular_coupon",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "regular_coupon_selected_product_variant",
                schema: "discount",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    regular_coupon_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_regular_coupon_selected_product_variant", x => x.id);
                    table.ForeignKey(
                        name: "fk_regular_coupon_selected_product_variant_regular_coupon_regu",
                        column: x => x.regular_coupon_id,
                        principalSchema: "discount",
                        principalTable: "regular_coupon",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "special_coupon_category",
                schema: "discount",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    special_coupon_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_special_coupon_category", x => x.id);
                    table.ForeignKey(
                        name: "fk_special_coupon_category_special_coupon_special_coupon_id",
                        column: x => x.special_coupon_id,
                        principalSchema: "discount",
                        principalTable: "special_coupon",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "special_coupon_selected_product_variant",
                schema: "discount",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    special_coupon_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_special_coupon_selected_product_variant", x => x.id);
                    table.ForeignKey(
                        name: "fk_special_coupon_selected_product_variant_special_coupon_spec",
                        column: x => x.special_coupon_id,
                        principalSchema: "discount",
                        principalTable: "special_coupon",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_regular_coupon_category_regular_coupon_id",
                schema: "discount",
                table: "regular_coupon_category",
                column: "regular_coupon_id");

            migrationBuilder.CreateIndex(
                name: "ix_regular_coupon_selected_product_variant_regular_coupon_id",
                schema: "discount",
                table: "regular_coupon_selected_product_variant",
                column: "regular_coupon_id");

            migrationBuilder.CreateIndex(
                name: "ix_special_coupon_category_special_coupon_id",
                schema: "discount",
                table: "special_coupon_category",
                column: "special_coupon_id");

            migrationBuilder.CreateIndex(
                name: "ix_special_coupon_selected_product_variant_special_coupon_id",
                schema: "discount",
                table: "special_coupon_selected_product_variant",
                column: "special_coupon_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "regular_coupon_category",
                schema: "discount");

            migrationBuilder.DropTable(
                name: "regular_coupon_selected_product_variant",
                schema: "discount");

            migrationBuilder.DropTable(
                name: "special_coupon_category",
                schema: "discount");

            migrationBuilder.DropTable(
                name: "special_coupon_selected_product_variant",
                schema: "discount");

            migrationBuilder.CreateTable(
                name: "coupon_category",
                schema: "discount",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    coupon_id = table.Column<Guid>(type: "uuid", nullable: false)
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
    }
}
