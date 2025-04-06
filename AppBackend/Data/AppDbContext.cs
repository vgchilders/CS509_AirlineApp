using Microsoft.EntityFrameworkCore;
using AppBackend.Models;



namespace AppBackend.Data
{
 public class AppDbContext:DbContext
 {
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options){

    }
    public DbSet<Deltas> Deltas{get;set;}
    public DbSet<SouthWests> SouthWests{get;set;}
    public DbSet<CombineFlight>CombineFlights{get;set;}
    public DbSet<ConnectingFlight>ConnectingFlights{get;set;}
    public DbSet<Cities>Cities{get;set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<CombineFlight>(entity=>{
            entity.HasNoKey();
            entity.ToView("combined_flights");
            entity.Property(e => e.Flight_source).HasColumnName("flight_source").HasColumnType("VARCHAR(20)");
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.DepartDateTime).HasColumnName("DepartDateTime").HasColumnType("DATETIME");
            entity.Property(e => e.ArriveDateTime).HasColumnName("ArriveDateTime").HasColumnType("DATETIME");
            entity.Property(e => e.DepartAirport).HasColumnName("DepartAirport").HasColumnType("VARCHAR(100)");
            entity.Property(e => e.ArriveAirport).HasColumnName("ArriveAirport").HasColumnType("VARCHAR(100)");
            entity.Property(e => e.FlightNumber).HasColumnName("FlightNumber").HasColumnType("VARCHAR(200)");
             entity.Property(e => e.Duration).HasColumnName("Duration");


        });
        modelBuilder.Entity<ConnectingFlight>(entity=>
        {
            entity.HasNoKey();
            entity.ToView("connecting_flights_view");
              entity.Property(e => e.Flight1_Id).HasColumnName("Flight1_Id");
                entity.Property(e => e.Flight2_Id).HasColumnName("Flight2_Id");
                entity.Property(e => e.Flight1_DepartAirport).HasColumnName("Flight1_DepartAirport").HasColumnType("VARCHAR(100)");
                entity.Property(e => e.ConnectingAirport).HasColumnName("ConnectingAirport").HasColumnType("VARCHAR(100)");
                entity.Property(e => e.Flight2_ArriveAirport).HasColumnName("Flight2_ArriveAirport").HasColumnType("VARCHAR(100)");
                entity.Property(e => e.Flight1_DepartureDateTime).HasColumnName("Flight1_DepartureDateTime").HasColumnType("DATETIME");
                entity.Property(e => e.Flight1_ArrivalDateTime).HasColumnName("Flight1_ArrivalDateTime").HasColumnType("DATETIME");
                entity.Property(e => e.Flight2_DepartureDateTime).HasColumnName("Flight2_DepartureDateTime").HasColumnType("DATETIME");
                entity.Property(e => e.Flight2_ArrivalDateTime).HasColumnName("Flight2_ArrivalDateTime").HasColumnType("DATETIME");
                entity.Property(e => e.ALLFlightDuration).HasColumnName("ALLFlightDuration");
                entity.Property(e => e.Flight1Duration).HasColumnName("Flight1Duration");
                entity.Property(e => e.Flight2Duration).HasColumnName("Flight2Duration");
                entity.Property(e => e.TransitTime).HasColumnName("TransitTime");
                entity.Property(e => e.IndirectFlightNumber).HasColumnName("IndirectFlightNumber").HasColumnType("VARCHAR(50)");
                entity.Property(e => e.Flight1_Source).HasColumnName("Flight1_Source").HasColumnType("VARCHAR(20)");
                entity.Property(e => e.Flight2_Source).HasColumnName("Flight2_Source").HasColumnType("VARCHAR(20)");

        }
        
        );
        
    }
    

    }

}