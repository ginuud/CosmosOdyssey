using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace REST.Migrations
{
    /// <inheritdoc />
    public partial class FinalFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteSegment_ReservedRoutes_ReservedRouteId",
                table: "RouteSegment");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteSegment_RouteInfos_RouteInfoId",
                table: "RouteSegment");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RouteSegment",
                table: "RouteSegment");

            migrationBuilder.RenameTable(
                name: "RouteSegment",
                newName: "RouteSegments");

            migrationBuilder.RenameIndex(
                name: "IX_RouteSegment_RouteInfoId",
                table: "RouteSegments",
                newName: "IX_RouteSegments_RouteInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_RouteSegment_ReservedRouteId",
                table: "RouteSegments",
                newName: "IX_RouteSegments_ReservedRouteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RouteSegments",
                table: "RouteSegments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteSegments_ReservedRoutes_ReservedRouteId",
                table: "RouteSegments",
                column: "ReservedRouteId",
                principalTable: "ReservedRoutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RouteSegments_RouteInfos_RouteInfoId",
                table: "RouteSegments",
                column: "RouteInfoId",
                principalTable: "RouteInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteSegments_ReservedRoutes_ReservedRouteId",
                table: "RouteSegments");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteSegments_RouteInfos_RouteInfoId",
                table: "RouteSegments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RouteSegments",
                table: "RouteSegments");

            migrationBuilder.RenameTable(
                name: "RouteSegments",
                newName: "RouteSegment");

            migrationBuilder.RenameIndex(
                name: "IX_RouteSegments_RouteInfoId",
                table: "RouteSegment",
                newName: "IX_RouteSegment_RouteInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_RouteSegments_ReservedRouteId",
                table: "RouteSegment",
                newName: "IX_RouteSegment_ReservedRouteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RouteSegment",
                table: "RouteSegment",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RouteSegment_ReservedRoutes_ReservedRouteId",
                table: "RouteSegment",
                column: "ReservedRouteId",
                principalTable: "ReservedRoutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RouteSegment_RouteInfos_RouteInfoId",
                table: "RouteSegment",
                column: "RouteInfoId",
                principalTable: "RouteInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
