using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace Modules.Catalog.Infra.Db.Contexts.CatalogDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Product_Search_View_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_product_variant_search_vector_turkish",
                schema: "catalog",
                table: "product_variant");

            migrationBuilder.DropIndex(
                name: "ix_product_attribute_search_vector_turkish",
                schema: "catalog",
                table: "product_attribute");

            migrationBuilder.DropIndex(
                name: "ix_product_search_vector_turkish",
                schema: "catalog",
                table: "product");

            migrationBuilder.DropColumn(
                name: "search_vector_turkish",
                schema: "catalog",
                table: "product_variant");

            migrationBuilder.DropColumn(
                name: "search_vector_turkish",
                schema: "catalog",
                table: "product_attribute");

            migrationBuilder.DropColumn(
                name: "search_vector_turkish",
                schema: "catalog",
                table: "product");

            migrationBuilder.Sql(@"
                CREATE VIEW catalog.""product_search_view"" AS
                select
	                p.id as ""product_id"",
	                to_tsvector('turkish', 
                        coalesce(p.""name"", '') || ' ' || 
                        coalesce(p.""description"", '') || ' ' || 
                        coalesce(p.""sku_value"", '') || ' ' ||
                        coalesce(c.""name"", '') || ' ' ||
                        coalesce(c.""slug_value"", '') || ' ' ||
                        coalesce(pv.""sku_value"", '') || ' ' ||
                        coalesce(pv.""slug_value"", '') || ' ' ||
                        coalesce(pa.""display_name"", '') || ' ' || 
                        coalesce(pa.""description"", '') || ' ' ||
                        coalesce(pa.""value"", '')
                    ) AS ""search_vector_turkish""
                from ""catalog"".product p
                left join ""catalog"".product_category pc on pc.product_id = p.id
                left join ""catalog"".category c on c.id = pc.category_id
                left join ""catalog"".product_variant pv on pv.product_id = p.id
                left join ""catalog"".product_attribute pa on pa.product_variant_id = pv.id
                where
	                p.is_deleted = false and
	                c.is_active = true and 
	                pv.is_deleted = false and pv.is_active = true;");

            //migrationBuilder.Sql(@"CREATE INDEX ""ix_product_search_view_search_vector_turkish"" ON catalog.""product_search_view"" USING GIN (""search_vector_turkish"");");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "search_vector_turkish",
                schema: "catalog",
                table: "product_variant",
                type: "tsvector",
                nullable: false,
                computedColumnSql: "to_tsvector('turkish', \"sku_value\")",
                stored: true);

            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "search_vector_turkish",
                schema: "catalog",
                table: "product_attribute",
                type: "tsvector",
                nullable: false,
                computedColumnSql: "to_tsvector('turkish', display_name || ' ' || value || ' ' || COALESCE(description, ''))",
                stored: true);

            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "search_vector_turkish",
                schema: "catalog",
                table: "product",
                type: "tsvector",
                nullable: false,
                computedColumnSql: "to_tsvector('turkish', \"name\" || ' ' || \"description\" || ' ' || \"sku_value\")",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "ix_product_variant_search_vector_turkish",
                schema: "catalog",
                table: "product_variant",
                column: "search_vector_turkish")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "ix_product_attribute_search_vector_turkish",
                schema: "catalog",
                table: "product_attribute",
                column: "search_vector_turkish")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "ix_product_search_vector_turkish",
                schema: "catalog",
                table: "product",
                column: "search_vector_turkish")
                .Annotation("Npgsql:IndexMethod", "GIN");

            //migrationBuilder.Sql(@"DROP INDEX IF EXISTS catalog.""ix_product_search_vector_turkish"";");

            migrationBuilder.Sql(@"DROP VIEW IF EXISTS catalog.""product_search_view"";");
        }
    }
}
