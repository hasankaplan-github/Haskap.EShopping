using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Account.Infra.Db.Contexts.AccountDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Phone_Number_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                schema: "account",
                table: "account",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "phone_number",
                schema: "account",
                table: "account");
        }
    }
}
