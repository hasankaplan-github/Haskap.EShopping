using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Catalog.Infra.Db.Contexts.CatalogDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Attributes_Renaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_color_attribute",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "product_size_attribute",
                schema: "catalog");

            migrationBuilder.CreateTable(
                name: "color_attribute",
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
                    table.PrimaryKey("pk_color_attribute", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "size_attribute",
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
                    table.PrimaryKey("pk_size_attribute", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "color_attribute",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "size_attribute",
                schema: "catalog");

            migrationBuilder.CreateTable(
                name: "product_color_attribute",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    display_name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false)
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
                    description = table.Column<string>(type: "text", nullable: true),
                    display_name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_size_attribute", x => x.id);
                });
        }
    }
}
