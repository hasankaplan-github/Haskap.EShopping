using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace Modules.Catalog.Infra.Db.Contexts.CatalogDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Attributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catalog");

            migrationBuilder.CreateTable(
                name: "category",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    slug_value = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    sku_value = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    search_vector_turkish = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false, computedColumnSql: "to_tsvector('turkish', \"name\" || ' ' || \"description\" || ' ' || \"sku_value\")", stored: true),
                    created_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_color_attribute",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_color_attribute", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_size_attribute",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_size_attribute", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_category",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_category", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_category_category_category_id",
                        column: x => x.category_id,
                        principalSchema: "catalog",
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_product_category_product_product_id",
                        column: x => x.product_id,
                        principalSchema: "catalog",
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_variant",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sku_value = table.Column<string>(type: "text", nullable: false),
                    old_price_value = table.Column<decimal>(type: "numeric", nullable: false),
                    old_price_currency = table.Column<string>(type: "text", nullable: false),
                    price_value = table.Column<decimal>(type: "numeric", nullable: false),
                    price_currency = table.Column<string>(type: "text", nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    stock_quantity_value = table.Column<int>(type: "integer", nullable: false),
                    search_vector_turkish = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false, computedColumnSql: "to_tsvector('turkish', \"sku_value\")", stored: true),
                    created_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_variant", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_variant_product_product_id",
                        column: x => x.product_id,
                        principalSchema: "catalog",
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_attribute",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    attribute_type = table.Column<string>(type: "text", nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    search_vector_turkish = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false, computedColumnSql: "to_tsvector('turkish', display_name || ' ' || value || ' ' || COALESCE(description, ''))", stored: true),
                    product_variant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_attribute", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_attribute_product_variant_product_variant_id",
                        column: x => x.product_variant_id,
                        principalSchema: "catalog",
                        principalTable: "product_variant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_picture",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    photo_file_id = table.Column<Guid>(type: "uuid", nullable: false),
                    photo_file_original_name = table.Column<string>(type: "text", nullable: false),
                    photo_file_new_name = table.Column<string>(type: "text", nullable: false),
                    photo_file_extension = table.Column<string>(type: "text", nullable: true),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false),
                    product_variant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_picture", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_picture_product_variant_product_variant_id",
                        column: x => x.product_variant_id,
                        principalSchema: "catalog",
                        principalTable: "product_variant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_category_name",
                schema: "catalog",
                table: "category",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_product_name",
                schema: "catalog",
                table: "product",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_product_search_vector_turkish",
                schema: "catalog",
                table: "product",
                column: "search_vector_turkish")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "ix_product_sku_value",
                schema: "catalog",
                table: "product",
                column: "sku_value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_product_attribute_product_variant_id",
                schema: "catalog",
                table: "product_attribute",
                column: "product_variant_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_attribute_search_vector_turkish",
                schema: "catalog",
                table: "product_attribute",
                column: "search_vector_turkish")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "ix_product_category_category_id",
                schema: "catalog",
                table: "product_category",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_category_product_id",
                schema: "catalog",
                table: "product_category",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_picture_product_variant_id",
                schema: "catalog",
                table: "product_picture",
                column: "product_variant_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_variant_product_id",
                schema: "catalog",
                table: "product_variant",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_variant_search_vector_turkish",
                schema: "catalog",
                table: "product_variant",
                column: "search_vector_turkish")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "ix_product_variant_sku_value",
                schema: "catalog",
                table: "product_variant",
                column: "sku_value",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_attribute",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "product_category",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "product_color_attribute",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "product_picture",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "product_size_attribute",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "category",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "product_variant",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "product",
                schema: "catalog");
        }
    }
}
