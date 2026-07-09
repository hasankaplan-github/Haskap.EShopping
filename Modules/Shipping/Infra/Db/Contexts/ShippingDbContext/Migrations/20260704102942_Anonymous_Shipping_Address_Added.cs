using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Shipping.Infra.Db.Contexts.ShippingDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Anonymous_Shipping_Address_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "anonymous_shipping_address",
                schema: "shipping",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    anonymous_basket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    city_id = table.Column<Guid>(type: "uuid", nullable: false),
                    district_id = table.Column<Guid>(type: "uuid", nullable: false),
                    neighborhood_id = table.Column<Guid>(type: "uuid", nullable: false),
                    street = table.Column<string>(type: "text", nullable: false),
                    postal_code = table.Column<string>(type: "text", nullable: true),
                    building_no = table.Column<string>(type: "text", nullable: false),
                    floor = table.Column<int>(type: "integer", nullable: true),
                    apartment_no = table.Column<int>(type: "integer", nullable: true),
                    address_line = table.Column<string>(type: "text", nullable: true)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "anonymous_shipping_address",
                schema: "shipping");
        }
    }
}
