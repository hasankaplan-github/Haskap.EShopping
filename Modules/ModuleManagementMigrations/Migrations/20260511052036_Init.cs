using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModuleManagementMigrations.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "module_management");

            migrationBuilder.CreateTable(
                name: "enabled_module",
                schema: "module_management",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_enabled_module", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_enabled_module_tenant_id",
                schema: "module_management",
                table: "enabled_module",
                column: "tenant_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "enabled_module",
                schema: "module_management");
        }
    }
}
