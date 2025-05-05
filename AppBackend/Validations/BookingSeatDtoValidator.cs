using AppBackend.DTOs;
using FluentValidation;

namespace AppBackend.Validations
{
 public class BookingDirectSeatsDtoValidator:AbstractValidator<BookingDirectSeatsDto>
 {
    public BookingDirectSeatsDtoValidator()
    {
        RuleFor(x=>x.FlightId).NotEmpty().GreaterThan(0);
        RuleFor(x=>x.FlightSource).NotEmpty()
         .Must(dir =>
         !string.IsNullOrWhiteSpace(dir) &&
         (dir.Equals("Delta", StringComparison.OrdinalIgnoreCase) || dir.Equals("Southwest", StringComparison.OrdinalIgnoreCase)))
         .WithMessage("source must be either 'Delta' or 'Southwest'.");
        RuleFor(x=>x.SeatNumber).NotEmpty();
        RuleFor(x=>x.SessionId).NotEmpty();
        RuleFor(x=>x.Direction).NotEmpty()
        .Must(dir=>dir=="outbound"|| dir=="return")
        .WithMessage("Direction must be either 'outbound' or 'return'.");
    }
 }

 public class BookingConnectingSeatsDtoValidator : AbstractValidator<BookingConnectingSeatsDto>
{
    public BookingConnectingSeatsDtoValidator()
    {
        RuleFor(x => x.Flight1Id).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Flight2Id).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Flight1Sournce).NotEmpty()
        .Must(dir =>
        !string.IsNullOrWhiteSpace(dir) &&
         (dir.Equals("Delta", StringComparison.OrdinalIgnoreCase) || dir.Equals("Southwest", StringComparison.OrdinalIgnoreCase)))
        .WithMessage("source must be either 'Delta' or 'Southwest'.").NotEmpty();
        RuleFor(x => x.Flight2Source).NotEmpty()
         .Must(dir =>
         !string.IsNullOrWhiteSpace(dir) &&
          (dir.Equals("Delta", StringComparison.OrdinalIgnoreCase) || dir.Equals("Southwest", StringComparison.OrdinalIgnoreCase)))
         .WithMessage("source must be either 'Delta' or 'Southwest'.");
        RuleFor(x => x.Seat1).NotEmpty();
        RuleFor(x => x.Seat2).NotEmpty();
        RuleFor(x => x.SessionId).NotEmpty();
        RuleFor(x => x.Direction).NotEmpty()
        .Must(dir=>dir=="outbound"|| dir=="return")
        .WithMessage("Direction must be either 'outbound' or 'return'.");
    }

}
}
