using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Shipping.Infra.Db.Contexts.ShippingDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Anonymous_Account : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "anonymous_shipping_address",
                schema: "shipping");

            migrationBuilder.RenameColumn(
                name: "postal_code",
                schema: "shipping",
                table: "account_shipping_address",
                newName: "postcode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "postcode",
                schema: "shipping",
                table: "account_shipping_address",
                newName: "postal_code");

            migrationBuilder.CreateTable(
                name: "anonymous_shipping_address",
                schema: "shipping",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    city_id = table.Column<Guid>(type: "uuid", nullable: false),
                    district_id = table.Column<Guid>(type: "uuid", nullable: false),
                    neighborhood_id = table.Column<Guid>(type: "uuid", nullable: false),
                    address_line = table.Column<string>(type: "text", nullable: true),
                    anonymous_basket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    apartment_no = table.Column<int>(type: "integer", nullable: true),
                    building_no = table.Column<string>(type: "text", nullable: false),
                    floor = table.Column<int>(type: "integer", nullable: true),
                    postcode = table.Column<string>(type: "text", nullable: true),
                    street = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_anonymous_shipping_address", x => x.id);
                    table.ForeignKey(
                        name: "fk_anonymous_shipping_address_city_city_id",
                        column: x => x.city_id,
                        principalSchema: "shipping",
                        principalTable: "city",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_anonymous_shipping_address_district_district_id",
                        column: x => x.district_id,
                        principalSchema: "shipping",
                        principalTable: "district",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_anonymous_shipping_address_neighborhood_neighborhood_id",
                        column: x => x.neighborhood_id,
                        principalSchema: "shipping",
                        principalTable: "neighborhood",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_anonymous_shipping_address_city_id",
                schema: "shipping",
                table: "anonymous_shipping_address",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_anonymous_shipping_address_district_id",
                schema: "shipping",
                table: "anonymous_shipping_address",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "ix_anonymous_shipping_address_neighborhood_id",
                schema: "shipping",
                table: "anonymous_shipping_address",
                column: "neighborhood_id");
        }
    }
}
