using AppBackend.Data;
using AppBackend.DTOs;
using AppBackend.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/v1/[controller]")]
public class CombinedFlightsController : ControllerBase
{
    private readonly IFlightRepository _flightRepository;
    private readonly ILogger<CombinedFlightsController> _logger;

    public CombinedFlightsController(IFlightRepository flightRepository, ILogger<CombinedFlightsController> logger)
    {
        _flightRepository = flightRepository;
        _logger = logger;
    }

    [HttpGet("search")]
    public async Task<ActionResult<FlightSearchResponses>> SearchFlights(
        string departAirport,
        string arriveAirport,
        DateTime departureDate,
        DateTime? returnDate = null)
    {
        _logger.LogInformation("Search initiated: {Depart} → {Arrive} on {DepartDate}, return: {ReturnDate}",
            departAirport,
            arriveAirport,
            departureDate.ToShortDateString(),
            returnDate?.ToShortDateString() ?? "none");

        try
        {
            var searchDto = new flightSearchDataDtos
            {
                ArrivalAirport = arriveAirport,
                DepartAirport = departAirport,
                Departuredate = departureDate,
                ReturnDate = returnDate,
            };

            var response = await _flightRepository.SearchFlights(searchDto);

            if (response == null ||
                (!response.DirectDepartFlights.Any() &&
                 !response.ConnectingDepartFlights.Any() &&
                 !response.DirectReturnFlights.Any() &&
                 !response.ConnectingReturnFlights.Any()))
            {
                _logger.LogWarning("No flights found for: {Depart} → {Arrive} on {DepartDate}",
                    departAirport,
                    arriveAirport,
                    departureDate.ToShortDateString());
            }
            else
            {
                _logger.LogInformation("Flights found: {DirectDepart} direct depart, {ConnectingDepart} connecting depart, {DirectReturn} direct return, {ConnectingReturn} connecting return",
                    response.DirectDepartFlights.Count(),
                    response.ConnectingDepartFlights.Count(),
                    response.DirectReturnFlights.Count(),
                    response.ConnectingReturnFlights.Count());
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while searching for flights: {Message}", ex.Message);
            return StatusCode(500, "An internal server error occurred while searching for flights.");
        }
    }
}
