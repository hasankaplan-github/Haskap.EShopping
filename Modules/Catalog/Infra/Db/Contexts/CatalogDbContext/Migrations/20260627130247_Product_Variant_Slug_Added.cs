using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Catalog.Infra.Db.Contexts.CatalogDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Product_Variant_Slug_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "slug_value",
                schema: "catalog",
                table: "product_variant",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_product_variant_slug_value",
                schema: "catalog",
                table: "product_variant",
                column: "slug_value",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_product_variant_slug_value",
                schema: "catalog",
                table: "product_variant");

            migrationBuilder.DropColumn(
                name: "slug_value",
                schema: "catalog",
                table: "product_variant");
        }
    }
}
