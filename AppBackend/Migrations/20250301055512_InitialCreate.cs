using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Deltas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DepartDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ArriveDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DepartAirport = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArriveAirport = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FlightNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deltas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SouthWests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DepartDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ArriveDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DepartAirport = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArriveAirport = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FlightNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SouthWests", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // Add sample data to the Deltas table
            migrationBuilder.InsertData(
                table: "Deltas",
                columns: new[] { "Id", "DepartDateTime", "ArriveDateTime", "DepartAirport", "ArriveAirport", "FlightNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 3, 1, 8, 0, 0), new DateTime(2023, 3, 1, 10, 0, 0), "JFK", "LAX", "DL100" },
                    { 2, new DateTime(2023, 3, 2, 9, 0, 0), new DateTime(2023, 3, 2, 11, 0, 0), "JFK", "LAX", "DL101" },
                    { 3, new DateTime(2023, 3, 3, 10, 0, 0), new DateTime(2023, 3, 3, 12, 0, 0), "JFK", "LAX", "DL102" }
                }
            );

            // Add sample data to the SouthWests table
            migrationBuilder.InsertData(
                table: "SouthWests",
                columns: new[] { "Id", "DepartDateTime", "ArriveDateTime", "DepartAirport", "ArriveAirport", "FlightNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 3, 1, 8, 0, 0), new DateTime(2023, 3, 1, 10, 0, 0), "JFK", "LAX", "SW100" },
                    { 2, new DateTime(2023, 3, 2, 9, 0, 0), new DateTime(2023, 3, 2, 11, 0, 0), "JFK", "LAX", "SW101" },
                    { 3, new DateTime(2023, 3, 3, 10, 0, 0), new DateTime(2023, 3, 3, 12, 0, 0), "JFK", "LAX", "SW102" }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deltas");

            migrationBuilder.DropTable(
                name: "SouthWests");
        }
    }
}
