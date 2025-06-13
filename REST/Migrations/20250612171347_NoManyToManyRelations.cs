using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace REST.Migrations
{
    /// <inheritdoc />
    public partial class NoManyToManyRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<string>>(
                name: "TransportationCompanyNames",
                table: "Reservations",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalQuotedTravelTime",
                table: "Reservations",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReservedRouteId",
                table: "Reservations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ReservedRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservedRoutes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RouteSegment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReservedRouteId = table.Column<int>(type: "integer", nullable: false),
                    RouteInfoId = table.Column<Guid>(type: "uuid", nullable: false),
                    SegmentOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteSegment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteSegment_ReservedRoutes_ReservedRouteId",
                        column: x => x.ReservedRouteId,
                        principalTable: "ReservedRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteSegment_RouteInfos_RouteInfoId",
                        column: x => x.RouteInfoId,
                        principalTable: "RouteInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ReservedRouteId",
                table: "Reservations",
                column: "ReservedRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteSegment_ReservedRouteId",
                table: "RouteSegment",
                column: "ReservedRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteSegment_RouteInfoId",
                table: "RouteSegment",
                column: "RouteInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_ReservedRoutes_ReservedRouteId",
                table: "Reservations",
                column: "ReservedRouteId",
                principalTable: "ReservedRoutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_ReservedRoutes_ReservedRouteId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "RouteSegment");

            migrationBuilder.DropTable(
                name: "ReservedRoutes");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ReservedRouteId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ReservedRouteId",
                table: "Reservations");

            migrationBuilder.AlterColumn<string>(
                name: "TransportationCompanyNames",
                table: "Reservations",
                type: "text",
                nullable: true,
                oldClrType: typeof(List<string>),
                oldType: "text[]");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TotalQuotedTravelTime",
                table: "Reservations",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);
        }
    }
}
