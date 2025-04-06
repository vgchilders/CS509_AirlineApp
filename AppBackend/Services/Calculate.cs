

using AppBackend.Interfaces;
using AppBackend.Repositories;

using GeoCoordinates.Core;
namespace  AppBackend.Services
{
    public class Calculate : Icalculate
    {
        private readonly ICitiesRepository _cityRepository;
        public Calculate(ICitiesRepository cityRepository){
            _cityRepository = cityRepository;
        }

        public async Task<double> CalaculatePrice(string city1, string city2)
        {  
            double basePrice=100;
            double perKmRate=0.02;

           var City1=await _cityRepository.GetCityByName(city1);
           var City2=await _cityRepository.GetCityByName(city2);
         
           var  coord1= new  Coordinate(City1.Latitude,City1.Longitude,0);
           var coord2= new Coordinate(City2.Latitude,City2.Longitude,0);
           double distanceInKilometers=coord1.GetDistanceTo(coord2)/1000;

           double price= basePrice+ (distanceInKilometers * perKmRate);
           return price;

        }
    }

    
}