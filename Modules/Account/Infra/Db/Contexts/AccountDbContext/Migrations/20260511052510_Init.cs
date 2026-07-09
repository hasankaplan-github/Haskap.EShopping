using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Account.Infra.Db.Contexts.AccountDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "account");

            migrationBuilder.CreateTable(
                name: "account",
                schema: "account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email_address = table.Column<string>(type: "text", nullable: true),
                    credentials_username = table.Column<string>(type: "text", nullable: false),
                    credentials_password_hashed_value = table.Column<string>(type: "text", nullable: false),
                    credentials_password_salt_value = table.Column<string>(type: "text", nullable: false),
                    is_locked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    login_attempt_failed_attempt_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    login_attempt_last_failed_attempt_utc_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                schema: "account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "account_permissions",
                schema: "account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account_permissions", x => x.id);
                    table.ForeignKey(
                        name: "fk_account_permissions_account_account_id",
                        column: x => x.account_id,
                        principalSchema: "account",
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "open_login",
                schema: "account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    utc_login_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    utc_last_seen_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    platform = table.Column<string>(type: "text", nullable: false),
                    browser = table.Column<string>(type: "text", nullable: false),
                    remote_ip_address = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_open_login", x => x.id);
                    table.ForeignKey(
                        name: "fk_open_login_account_account_id",
                        column: x => x.account_id,
                        principalSchema: "account",
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_role",
                schema: "account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account_role", x => x.id);
                    table.ForeignKey(
                        name: "fk_account_role_account_account_id",
                        column: x => x.account_id,
                        principalSchema: "account",
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_account_role_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "account",
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                schema: "account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permissions", x => x.id);
                    table.ForeignKey(
                        name: "fk_role_permissions_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "account",
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_account_credentials_username",
                schema: "account",
                table: "account",
                column: "credentials_username");

            migrationBuilder.CreateIndex(
                name: "ix_account_permissions_account_id",
                schema: "account",
                table: "account_permissions",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_role_account_id",
                schema: "account",
                table: "account_role",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_role_role_id",
                schema: "account",
                table: "account_role",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_open_login_account_id_id",
                schema: "account",
                table: "open_login",
                columns: new[] { "account_id", "id" });

            migrationBuilder.CreateIndex(
                name: "ix_role_permissions_role_id",
                schema: "account",
                table: "role_permissions",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_permissions",
                schema: "account");

            migrationBuilder.DropTable(
                name: "account_role",
                schema: "account");

            migrationBuilder.DropTable(
                name: "open_login",
                schema: "account");

            migrationBuilder.DropTable(
                name: "role_permissions",
                schema: "account");

            migrationBuilder.DropTable(
                name: "account",
                schema: "account");

            migrationBuilder.DropTable(
                name: "role",
                schema: "account");
        }
    }
}
