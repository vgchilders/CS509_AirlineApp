using AppBackend.Data;
using AppBackend.Interfaces;
using AppBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AppBackend.Repositories
{

    public class CityRepository : ICitiesRepository
    {
     private readonly AppDbContext _context;
     public CityRepository(AppDbContext context){
        _context=context;
     }
        public async Task<Cities> GetCityByName(string cityName)
        {
            var city= await _context.Cities.FirstOrDefaultAsync(c=>c.CityName.Contains(cityName));
            if (city==null) {
                throw new ArgumentException("city not found");
            }
            return city;
                }
}
}