using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Shipping.Infra.Db.Contexts.ShippingDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "shipping");

            migrationBuilder.CreateTable(
                name: "city",
                schema: "shipping",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_city", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shipping_fee",
                schema: "shipping",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    city_id = table.Column<Guid>(type: "uuid", nullable: false),
                    district_id = table.Column<Guid>(type: "uuid", nullable: false),
                    neighborhood_id = table.Column<Guid>(type: "uuid", nullable: false),
                    price_value = table.Column<decimal>(type: "numeric", nullable: false),
                    price_currency = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shipping_fee", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "district",
                schema: "shipping",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    city_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_district", x => x.id);
                    table.ForeignKey(
                        name: "fk_district_city_city_id",
                        column: x => x.city_id,
                        principalSchema: "shipping",
                        principalTable: "city",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "neighborhood",
                schema: "shipping",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    district_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_neighborhood", x => x.id);
                    table.ForeignKey(
                        name: "fk_neighborhood_district_district_id",
                        column: x => x.district_id,
                        principalSchema: "shipping",
                        principalTable: "district",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account_shipping_address",
                schema: "shipping",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_account_id = table.Column<Guid>(type: "uuid", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "ix_district_city_id",
                schema: "shipping",
                table: "district",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_neighborhood_district_id",
                schema: "shipping",
                table: "neighborhood",
                column: "district_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_shipping_address",
                schema: "shipping");

            migrationBuilder.DropTable(
                name: "shipping_fee",
                schema: "shipping");

            migrationBuilder.DropTable(
                name: "neighborhood",
                schema: "shipping");

            migrationBuilder.DropTable(
                name: "district",
                schema: "shipping");

            migrationBuilder.DropTable(
                name: "city",
                schema: "shipping");
        }
    }
}
