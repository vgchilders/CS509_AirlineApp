using AppBackend.DTOs;
using FluentValidation;

namespace AppBackend.Validations
{
public class BookingInfoDtoValidator:AbstractValidator<BookingInfoDto>
{
    public BookingInfoDtoValidator()
    {

     RuleFor(x => x.SessionId)
            .NotEmpty().WithMessage("Session ID is required");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .Matches(@"^[A-Za-z]").WithMessage("First name start With a latter.")
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .Matches(@"^[A-Za-z]").WithMessage("Last name start With a latter.")
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches("^\\d{10,15}$").WithMessage("Phone number must be 10 to 15 digits");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0");

        RuleFor(x => x.Flights)
            .NotEmpty().WithMessage("At least one flight must be selected")
            .ForEach(flightRule => flightRule.SetValidator(new FlightLegDtoValidator()));

    }
}
public class FlightLegDtoValidator : AbstractValidator<FlightLegDto>
{
    public FlightLegDtoValidator()
    {
        RuleFor(x => x.FlightId)
            .GreaterThan(0).WithMessage("Flight ID must be greater than 0");

        RuleFor(x => x.FlightSource)
          .Must(dir => dir.Equals("Delta", StringComparison.OrdinalIgnoreCase) || dir.Equals("Southwest", StringComparison.OrdinalIgnoreCase))

            .NotEmpty().WithMessage("Flight source is required");

        RuleFor(x => x.Direction)
          .Must(dir => dir.Equals("outbound", StringComparison.OrdinalIgnoreCase) || dir.Equals("return", StringComparison.OrdinalIgnoreCase))
            .NotEmpty().WithMessage("Flight direction is required");
    }
}




}
