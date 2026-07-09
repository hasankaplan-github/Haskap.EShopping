using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Catalog.Infra.Db.Contexts.CatalogDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Attribute_Type_Pair_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_attribute_type_for_details",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    attribute_type = table.Column<string>(type: "text", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_attribute_type_for_details", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_attribute_type_for_details_product_product_id",
                        column: x => x.product_id,
                        principalSchema: "catalog",
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_product_attribute_type_for_details_product_id",
                schema: "catalog",
                table: "product_attribute_type_for_details",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_attribute_type_for_details",
                schema: "catalog");
        }
    }
}
