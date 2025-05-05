using FluentValidation;
using System;

namespace AppBackend.DTOs.Validators
{
    public class FlightSearchDataDtosValidator : AbstractValidator<flightSearchDataDtos>
    {
        public FlightSearchDataDtosValidator()
        {
            RuleFor(x => x.DepartAirport)
                .NotEmpty().WithMessage("Departure airport is required.")
                .MaximumLength(10).WithMessage("Departure airport code is too long.");

            RuleFor(x => x.ArrivalAirport)
                .NotEmpty().WithMessage("Arrival airport is required.")
                .MaximumLength(10).WithMessage("Arrival airport code is too long.")
                .NotEqual(x => x.DepartAirport)
                .WithMessage("Arrival airport cannot be the same as departure airport.");

            RuleFor(x => x.Departuredate)
                .NotEmpty()
                .WithMessage("Departure date cannot be in the past.");

            RuleFor(x => x.ReturnDate)
                .GreaterThan(x => x.Departuredate)
                .When(x => x.ReturnDate.HasValue)
                .WithMessage("Return date must be after the departure date.");
        }
    }
}
