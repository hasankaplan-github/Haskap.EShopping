using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Catalog.Infra.Db.Contexts.CatalogDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Product_Picture_Configuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "photo_file_id",
                schema: "catalog",
                table: "product_picture");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "photo_file_id",
                schema: "catalog",
                table: "product_picture",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
