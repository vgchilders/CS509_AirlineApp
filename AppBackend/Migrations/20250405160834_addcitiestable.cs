using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;
using System.Reflection;
#nullable disable

namespace AppBackend.Migrations
{
    /// <inheritdoc />
    public partial class addcitiestable : Migration
    {
        /// <inheritdoc /> 
             private const string SqlFileName="AppBackend.Data.cities.sql";
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id = table.Column<int>(type: "int", nullable: false).
                    Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Longitude = table.Column<float>(type: "float", nullable: false),
                    Latitude = table.Column<float>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
                var assembly= Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream(SqlFileName)){
                    if (stream != null){
                        using (var reader = new StreamReader(stream)) {
                            var sqlScript=reader.ReadToEnd();
                            var commands=sqlScript.Split(new []{';'},StringSplitOptions.RemoveEmptyEntries);
                            foreach (var command in commands)
                            {  var trimmed = command.Trim();
                              if (!string.IsNullOrWhiteSpace(trimmed))
                              {
                              Console.WriteLine("Executing SQL:");
                              Console.WriteLine(trimmed);
                              migrationBuilder.Sql(trimmed);
                                }
                            }
                        }
                    }
                    else{
                       Console.WriteLine($"SQL script file not found: {SqlFileName}"); 
                    }
                }   
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
