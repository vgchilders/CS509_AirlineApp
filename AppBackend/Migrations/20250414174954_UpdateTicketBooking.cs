using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTicketBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicketBookingFlight",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FlightId = table.Column<int>(type: "int", nullable: false),
                    FlightSource = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TicketBookingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketBookingFlight", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketBookingFlight_TicketBookings_TicketBookingId",
                        column: x => x.TicketBookingId,
                        principalTable: "TicketBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TicketBookingFlight_TicketBookingId",
                table: "TicketBookingFlight",
                column: "TicketBookingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketBookingFlight");
        }
    }
}
