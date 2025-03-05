using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBackend.Migrations
{
    /// <inheritdoc />
    public partial class CreateConnectingFlightsView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         migrationBuilder.Sql(@"
            CREATE VIEW connecting_flights_view AS
            SELECT 
                f1.Id AS Flight1_Id,
                f2.Id AS Flight2_Id,
                f1.DepartAirport AS Flight1_DepartAirport,
                f1.ArriveAirport AS ConnectingAirport,
                f2.ArriveAirport AS Flight2_ArriveAirport,
                f1.DepartDateTime AS Flight1_DepartureDateTime,
                f1.ArriveDateTime AS Flight1_ArrivalDateTime,
                f2.DepartDateTime AS Flight2_DepartureDateTime,
                f2.ArriveDateTime AS Flight2_ArrivalDateTime,
                TIMEDIFF(f2.ArriveDateTime, f1.DepartDateTime) AS ALLFlightDuration,
                TIMEDIFF(f2.DepartDateTime, f1.ArriveDateTime) AS TransitTime,
                CONCAT(f1.FlightNumber, '-', f2.FlightNumber) AS IndirectFlightNumber,
                f1.Flight_source AS Flight1_Source,
                f2.Flight_source AS Flight2_Source
            FROM combined_flights f1
            JOIN combined_flights f2 
                ON f1.ArriveAirport = f2.DepartAirport
            WHERE f1.ArriveDateTime < f2.DepartDateTime
                AND f1.DepartAirport <> f2.ArriveAirport
                AND f2.DepartDateTime >= DATE_ADD(f1.ArriveDateTime, INTERVAL 30 MINUTE)
                AND f2.DepartDateTime <= DATE_ADD(f1.ArriveDateTime, INTERVAL 6 HOUR);
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
         migrationBuilder.Sql("DROP VIEW IF EXISTS connecting_flights_view");
        }
    }
}
