using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Basket.Infra.Db.Contexts.BasketDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "basket");

            migrationBuilder.CreateTable(
                name: "basket",
                schema: "basket",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_account_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_basket", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "basket_item",
                schema: "basket",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    basket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_basket_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_basket_item_basket_basket_id",
                        column: x => x.basket_id,
                        principalSchema: "basket",
                        principalTable: "basket",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_basket_item_basket_id",
                schema: "basket",
                table: "basket_item",
                column: "basket_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "basket_item",
                schema: "basket");

            migrationBuilder.DropTable(
                name: "basket",
                schema: "basket");
        }
    }
}
