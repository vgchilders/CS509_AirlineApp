using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBackend.Migrations
{
    /// <inheritdoc />
    public partial class updateCombinedFlightviewWithBetterTimeFormat : Migration
    {
        /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
        
        {   migrationBuilder.Sql("DROP VIEW IF EXISTS combined_flights;");
            migrationBuilder.Sql(@"
    CREATE VIEW combined_flights AS
    SELECT 
        'Delta' AS flight_source,
        Id,
        DepartDateTime,
        ArriveDateTime,
        DepartAirport,
        ArriveAirport,
        FlightNumber,
        TIMEDIFF(ArriveDateTime, DepartDateTime) AS Duration
    FROM deltas
    UNION ALL
    SELECT 
        'Southwest' AS flight_source,
        Id,
        DepartDateTime,
        ArriveDateTime,
        DepartAirport,
        ArriveAirport,
        FlightNumber,
         TIMEDIFF(ArriveDateTime, DepartDateTime) AS Duration
    FROM southwests;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.Sql("DROP VIEW IF EXISTS combined_flights;");
        }
    }
}
