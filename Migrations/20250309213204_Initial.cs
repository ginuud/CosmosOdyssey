using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmosOdyssey.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Planet",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pricelists",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pricelists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RouteInfos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FromId = table.Column<string>(type: "text", nullable: false),
                    ToId = table.Column<string>(type: "text", nullable: false),
                    Distance = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteInfos_Planet_FromId",
                        column: x => x.FromId,
                        principalTable: "Planet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RouteInfos_Planet_ToId",
                        column: x => x.ToId,
                        principalTable: "Planet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Legs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    RouteInfoId = table.Column<string>(type: "text", nullable: false),
                    PricelistId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Legs_Pricelists_PricelistId",
                        column: x => x.PricelistId,
                        principalTable: "Pricelists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Legs_RouteInfos_RouteInfoId",
                        column: x => x.RouteInfoId,
                        principalTable: "RouteInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    FlightStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LegId = table.Column<string>(type: "text", nullable: false),
                    FlightEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Providers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Providers_Legs_LegId",
                        column: x => x.LegId,
                        principalTable: "Legs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Legs_PricelistId",
                table: "Legs",
                column: "PricelistId");

            migrationBuilder.CreateIndex(
                name: "IX_Legs_RouteInfoId",
                table: "Legs",
                column: "RouteInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_CompanyId",
                table: "Providers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_LegId",
                table: "Providers",
                column: "LegId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteInfos_FromId",
                table: "RouteInfos",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteInfos_ToId",
                table: "RouteInfos",
                column: "ToId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Legs");

            migrationBuilder.DropTable(
                name: "Pricelists");

            migrationBuilder.DropTable(
                name: "RouteInfos");

            migrationBuilder.DropTable(
                name: "Planet");
        }
    }
}
