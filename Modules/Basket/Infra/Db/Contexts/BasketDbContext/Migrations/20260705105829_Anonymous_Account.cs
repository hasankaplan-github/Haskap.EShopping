using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Basket.Infra.Db.Contexts.BasketDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Anonymous_Account : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "owner_account_id",
                schema: "basket",
                table: "basket",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "owner_account_id",
                schema: "basket",
                table: "basket",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
