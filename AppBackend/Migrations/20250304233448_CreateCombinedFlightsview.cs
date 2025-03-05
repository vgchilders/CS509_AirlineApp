using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBackend.Migrations
{
    /// <inheritdoc />
    public partial class CreateCombinedFlightsview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
 migrationBuilder.Sql(@"
            CREATE VIEW combined_flights AS
            SELECT 
                'Delta' AS flight_source,
                Id,
                DepartDateTime,
                ArriveDateTime,
                DepartAirport,
                ArriveAirport,
                FlightNumber
            FROM deltas
            UNION ALL
            SELECT 
                'Southwest' AS flight_source,
                Id,
                DepartDateTime,
                ArriveDateTime,
                DepartAirport,
                ArriveAirport,
                FlightNumber
            FROM southwests;
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
         migrationBuilder.Sql("Drop VIEW IF EXISTES combined_flights");
         }
    }
}
