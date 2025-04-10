using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace REST.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
        name: "Companies",
        columns: table => new
        {
            Id = table.Column<Guid>(type: "uuid", nullable: false),
            Name = table.Column<string>(type: "text", nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_Companies", x => x.Id);
        });

            migrationBuilder.CreateTable(
                name: "Planets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "timestamp(7) with time zone", precision: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RouteInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToId = table.Column<Guid>(type: "uuid", nullable: false),
                    Distance = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteInfos_Planets_FromId",
                        column: x => x.FromId,
                        principalTable: "Planets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RouteInfos_Planets_ToId",
                        column: x => x.ToId,
                        principalTable: "Planets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Legs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RouteInfoId = table.Column<Guid>(type: "uuid", nullable: false),
                    PriceListId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Legs_PriceLists_PriceListId",
                        column: x => x.PriceListId,
                        principalTable: "PriceLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Legs_RouteInfos_RouteInfoId",
                        column: x => x.RouteInfoId,
                        principalTable: "RouteInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    FlightStart = table.Column<DateTime>(type: "timestamp(7) with time zone", precision: 7, nullable: false),
                    FlightEnd = table.Column<DateTime>(type: "timestamp(7) with time zone", precision: 7, nullable: false),
                    LegId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Providers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Providers_Legs_LegId",
                        column: x => x.LegId,
                        principalTable: "Legs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    TotalQuotedPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    TotalQuotedTravelTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TransportationCompanyNames = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                });

            migrationBuilder.Sql("ALTER TABLE \"RouteInfos\" ALTER COLUMN \"Id\" TYPE uuid USING \"Id\"::uuid;");
            migrationBuilder.Sql("ALTER TABLE \"RouteInfos\" ALTER COLUMN \"ToId\" TYPE uuid USING \"ToId\"::uuid;");
            migrationBuilder.Sql("ALTER TABLE \"RouteInfos\" ALTER COLUMN \"FromId\" TYPE uuid USING \"FromId\"::uuid;");
            migrationBuilder.Sql("ALTER TABLE \"Providers\" ALTER COLUMN \"Id\" TYPE uuid USING \"Id\"::uuid;");
            migrationBuilder.Sql("ALTER TABLE \"Providers\" ALTER COLUMN \"CompanyId\" TYPE uuid USING \"CompanyId\"::uuid;");
            migrationBuilder.Sql("ALTER TABLE \"Providers\" ALTER COLUMN \"LegId\" TYPE uuid USING \"LegId\"::uuid;");
            migrationBuilder.Sql("ALTER TABLE \"PriceLists\" ALTER COLUMN \"Id\" TYPE uuid USING \"Id\"::uuid;");
            migrationBuilder.Sql("ALTER TABLE \"Planets\" ALTER COLUMN \"Id\" TYPE uuid USING \"Id\"::uuid;");
            migrationBuilder.Sql("ALTER TABLE \"Legs\" ALTER COLUMN \"Id\" TYPE uuid USING \"Id\"::uuid;");
            migrationBuilder.Sql("ALTER TABLE \"Legs\" ALTER COLUMN \"RouteInfoId\" TYPE uuid USING \"RouteInfoId\"::uuid;");
            migrationBuilder.Sql("ALTER TABLE \"Legs\" ALTER COLUMN \"PriceListId\" TYPE uuid USING \"PriceListId\"::uuid;");
            migrationBuilder.Sql("ALTER TABLE \"Companies\" ALTER COLUMN \"Id\" TYPE uuid USING \"Id\"::uuid;");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Legs");

            migrationBuilder.DropTable(
                name: "PriceLists");

            migrationBuilder.DropTable(
                name: "RouteInfos");

            migrationBuilder.DropTable(
                name: "Planets");

        }
    }
}
