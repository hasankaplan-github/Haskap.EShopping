using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Shipping.Infra.Db.Contexts.ShippingDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Shipping_Address_Renaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_shipping_address",
                schema: "shipping");

            migrationBuilder.CreateTable(
                name: "shipping_address",
                schema: "shipping",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    city_id = table.Column<Guid>(type: "uuid", nullable: false),
                    district_id = table.Column<Guid>(type: "uuid", nullable: false),
                    neighborhood_id = table.Column<Guid>(type: "uuid", nullable: false),
                    street = table.Column<string>(type: "text", nullable: false),
                    postcode = table.Column<string>(type: "text", nullable: true),
                    building_no = table.Column<string>(type: "text", nullable: false),
                    floor = table.Column<int>(type: "integer", nullable: true),
                    apartment_no = table.Column<int>(type: "integer", nullable: true),
                    address_line = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shipping_address", x => x.id);
                    table.ForeignKey(
                        name: "fk_shipping_address_city_city_id",
                        column: x => x.city_id,
                        principalSchema: "shipping",
                        principalTable: "city",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_shipping_address_district_district_id",
                        column: x => x.district_id,
                        principalSchema: "shipping",
                        principalTable: "district",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_shipping_address_neighborhood_neighborhood_id",
                        column: x => x.neighborhood_id,
                        principalSchema: "shipping",
                        principalTable: "neighborhood",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_shipping_address_city_id",
                schema: "shipping",
                table: "shipping_address",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_shipping_address_district_id",
                schema: "shipping",
                table: "shipping_address",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "ix_shipping_address_neighborhood_id",
                schema: "shipping",
                table: "shipping_address",
                column: "neighborhood_id");

            migrationBuilder.CreateIndex(
                name: "ix_shipping_address_owner_account_id",
                schema: "shipping",
                table: "shipping_address",
                column: "owner_account_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shipping_address",
                schema: "shipping");

            migrationBuilder.CreateTable(
                name: "account_shipping_address",
                schema: "shipping",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    city_id = table.Column<Guid>(type: "uuid", nullable: false),
                    district_id = table.Column<Guid>(type: "uuid", nullable: false),
                    neighborhood_id = table.Column<Guid>(type: "uuid", nullable: false),
                    address_line = table.Column<string>(type: "text", nullable: true),
                    apartment_no = table.Column<int>(type: "integer", nullable: true),
                    building_no = table.Column<string>(type: "text", nullable: false),
                    floor = table.Column<int>(type: "integer", nullable: true),
                    owner_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    postcode = table.Column<string>(type: "text", nullable: true),
                    street = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account_shipping_address", x => x.id);
                    table.ForeignKey(
                        name: "fk_account_shipping_address_city_city_id",
                        column: x => x.city_id,
                        principalSchema: "shipping",
                        principalTable: "city",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_account_shipping_address_district_district_id",
                        column: x => x.district_id,
                        principalSchema: "shipping",
                        principalTable: "district",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_account_shipping_address_neighborhood_neighborhood_id",
                        column: x => x.neighborhood_id,
                        principalSchema: "shipping",
                        principalTable: "neighborhood",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_account_shipping_address_city_id",
                schema: "shipping",
                table: "account_shipping_address",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_shipping_address_district_id",
                schema: "shipping",
                table: "account_shipping_address",
                column: "district_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_shipping_address_neighborhood_id",
                schema: "shipping",
                table: "account_shipping_address",
                column: "neighborhood_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_shipping_address_owner_account_id",
                schema: "shipping",
                table: "account_shipping_address",
                column: "owner_account_id");
        }
    }
}
