using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppBackend.Migrations
{
    /// <inheritdoc />
    public partial class updateDeltasAndSouthwest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE deltas
                SET ArriveDateTime = DATE_ADD(ArriveDateTime, INTERVAL 1 DAY)
                WHERE DepartDateTime > ArriveDateTime;
            ");

            // Fix Southwest flights where arrival is before departure
            migrationBuilder.Sql(@"
                UPDATE southwests
                SET ArriveDateTime = DATE_ADD(ArriveDateTime, INTERVAL 1 DAY)
                WHERE DepartDateTime > ArriveDateTime;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
