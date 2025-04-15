using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddDirectionToTicketBookingFligh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketBookingFlight_TicketBookings_TicketBookingId",
                table: "TicketBookingFlight");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketBookingFlight",
                table: "TicketBookingFlight");

            migrationBuilder.RenameTable(
                name: "TicketBookingFlight",
                newName: "TicketBookingFlights");

            migrationBuilder.RenameIndex(
                name: "IX_TicketBookingFlight_TicketBookingId",
                table: "TicketBookingFlights",
                newName: "IX_TicketBookingFlights_TicketBookingId");

            migrationBuilder.AddColumn<string>(
                name: "Direction",
                table: "TicketBookingFlights",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketBookingFlights",
                table: "TicketBookingFlights",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketBookingFlights_TicketBookings_TicketBookingId",
                table: "TicketBookingFlights",
                column: "TicketBookingId",
                principalTable: "TicketBookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketBookingFlights_TicketBookings_TicketBookingId",
                table: "TicketBookingFlights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketBookingFlights",
                table: "TicketBookingFlights");

            migrationBuilder.DropColumn(
                name: "Direction",
                table: "TicketBookingFlights");

            migrationBuilder.RenameTable(
                name: "TicketBookingFlights",
                newName: "TicketBookingFlight");

            migrationBuilder.RenameIndex(
                name: "IX_TicketBookingFlights_TicketBookingId",
                table: "TicketBookingFlight",
                newName: "IX_TicketBookingFlight_TicketBookingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketBookingFlight",
                table: "TicketBookingFlight",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketBookingFlight_TicketBookings_TicketBookingId",
                table: "TicketBookingFlight",
                column: "TicketBookingId",
                principalTable: "TicketBookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
