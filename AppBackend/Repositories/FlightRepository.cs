using AppBackend.Data;
using AppBackend.DTOs;
using AppBackend.Interfaces;
using AppBackend.Mapping;
using AppBackend.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppBackend.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        private readonly IPricecalculate _calculate;
        public FlightRepository(AppDbContext context, IMapper mapper,IPricecalculate calculate){
            _context = context;
            _mapper= mapper;

            _calculate= calculate;

        }
        public async Task<FlightSearchResponses> SearchFlights(flightSearchDataDtos flightSearch)
        {

        //  Fetch direct one-way flights
         var directDepartFlights = await _context.CombineFlights
            .Where(f => f.DepartAirport.Contains( flightSearch.DepartAirport)
                        && f.ArriveAirport.Contains(flightSearch.ArrivalAirport)
                        && f.DepartDateTime.Date == flightSearch.Departuredate.Date)
            .ToListAsync();

        var directDepartFlightDtos = _mapper.Map<IEnumerable<CombinedFlightDto>>(directDepartFlights);
        double price= await _calculate.CalaculatePrice(flightSearch.DepartAirport,flightSearch.ArrivalAirport);

        foreach (var flight in directDepartFlightDtos)
        {
            flight.Price=price;

        }

        //  Fetch connecting one-way flights
        var connectingDepartFlights = await _context.ConnectingFlights
            .Where(f => f.Flight1_DepartAirport.Contains( flightSearch.DepartAirport)
                        && f.Flight2_ArriveAirport.Contains(flightSearch.ArrivalAirport)
                        && f.Flight1_DepartureDateTime.Date == flightSearch.Departuredate.Date)
            .ToListAsync();

        var connectingDepartFlightDtos = _mapper.Map<IEnumerable<ConnectingFlightDto>>(connectingDepartFlights);
               foreach (var flight in connectingDepartFlightDtos)
        {
            flight.Price=price;

        }

        //  Combine direct & connecting flights
        var response = new FlightSearchResponses{
         DirectDepartFlights=directDepartFlightDtos,
         ConnectingDepartFlights=connectingDepartFlightDtos


        };


        //  Fetch round-trip flights if `returnDate` is provided
        var returnFlights = new List<object>();

        if (flightSearch.ReturnDate.HasValue)
        {
            var directReturnFlights = await _context.CombineFlights
                .Where(f => f.DepartAirport.Contains(flightSearch.ArrivalAirport)
                            && f.ArriveAirport.Contains(flightSearch.DepartAirport)
                            && f.DepartDateTime.Date == flightSearch.ReturnDate.Value.Date)
                .ToListAsync();

            var directReturnFlightDtos = _mapper.Map<IEnumerable<CombinedFlightDto>>(directReturnFlights);
            foreach (var flight in directReturnFlightDtos)
                {
                   flight.Price=price;

                   }

            var connectingReturnFlights = await _context.ConnectingFlights
                .Where(f => f.Flight1_DepartAirport.Contains(flightSearch.ArrivalAirport)
                            && f.Flight2_ArriveAirport.Contains(flightSearch.DepartAirport)
                            && f.Flight1_DepartureDateTime.Date == flightSearch.ReturnDate.Value.Date)
                .ToListAsync();

            var connectingReturnFlightDtos = _mapper.Map<IEnumerable<ConnectingFlightDto>>(connectingReturnFlights);
             foreach (var flight in connectingReturnFlightDtos)
        {
            flight.Price=price;

        }
            response.DirectReturnFlights=directReturnFlightDtos;
            response.ConnectingReturnFlights=connectingReturnFlightDtos;
        }

        return response;
          }




        }
    }
