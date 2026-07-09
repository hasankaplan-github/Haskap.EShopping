using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Order.Infra.Db.Contexts.OrderDbContext.Migrations
{
    /// <inheritdoc />
    public partial class IdempotencyKey_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "idempotency_key",
                schema: "order",
                table: "order",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_order_idempotency_key",
                schema: "order",
                table: "order",
                column: "idempotency_key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_order_idempotency_key",
                schema: "order",
                table: "order");

            migrationBuilder.DropColumn(
                name: "idempotency_key",
                schema: "order",
                table: "order");
        }
    }
}
