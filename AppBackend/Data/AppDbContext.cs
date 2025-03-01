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
 

    }

}