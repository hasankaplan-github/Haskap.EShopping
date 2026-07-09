using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Shipping.Infra.Db.Contexts.ShippingDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Renaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "postal_code",
                schema: "shipping",
                table: "anonymous_shipping_address",
                newName: "postcode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "postcode",
                schema: "shipping",
                table: "anonymous_shipping_address",
                newName: "postal_code");
        }
    }
}
