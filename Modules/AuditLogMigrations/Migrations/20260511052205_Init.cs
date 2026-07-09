using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditLogMigrations.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "audit_log");

            migrationBuilder.CreateTable(
                name: "audit_history_log",
                schema: "audit_log",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    visit_id = table.Column<Guid>(type: "uuid", nullable: true),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modification_date_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modification_type = table.Column<string>(type: "text", nullable: false),
                    object_full_type = table.Column<string>(type: "text", nullable: false),
                    object_ids = table.Column<string>(type: "text", nullable: true),
                    object_original_values = table.Column<string>(type: "text", nullable: true),
                    object_new_values = table.Column<string>(type: "text", nullable: true),
                    ownership_type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_history_log", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_history_log",
                schema: "audit_log");
        }
    }
}
