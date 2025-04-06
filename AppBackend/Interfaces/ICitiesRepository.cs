using AppBackend.Models;

namespace AppBackend.Interfaces
{
 public interface ICitiesRepository{
    Task<Cities> GetCityByName(string cityName);
 }   
}