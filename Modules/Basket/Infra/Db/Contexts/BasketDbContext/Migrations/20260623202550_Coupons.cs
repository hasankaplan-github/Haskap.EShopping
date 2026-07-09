using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Basket.Infra.Db.Contexts.BasketDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Coupons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "basket_applied_special_coupon",
                schema: "basket",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    basket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    special_coupon_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_basket_applied_special_coupon", x => x.id);
                    table.ForeignKey(
                        name: "fk_basket_applied_special_coupon_basket_basket_id",
                        column: x => x.basket_id,
                        principalSchema: "basket",
                        principalTable: "basket",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "basket_not_applied_regular_coupon",
                schema: "basket",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    basket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    regular_coupon_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_basket_not_applied_regular_coupon", x => x.id);
                    table.ForeignKey(
                        name: "fk_basket_not_applied_regular_coupon_basket_basket_id",
                        column: x => x.basket_id,
                        principalSchema: "basket",
                        principalTable: "basket",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_basket_applied_special_coupon_basket_id",
                schema: "basket",
                table: "basket_applied_special_coupon",
                column: "basket_id");

            migrationBuilder.CreateIndex(
                name: "ix_basket_not_applied_regular_coupon_basket_id",
                schema: "basket",
                table: "basket_not_applied_regular_coupon",
                column: "basket_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "basket_applied_special_coupon",
                schema: "basket");

            migrationBuilder.DropTable(
                name: "basket_not_applied_regular_coupon",
                schema: "basket");
        }
    }
}
