using FluentValidation;
using AppBackend.DTOs;

namespace AppBackend.Validations
{
    public class GetDirectFlightSeatRequestDtoValidator : AbstractValidator<GetDirectFlightSeatRequestDto>
    {
        public GetDirectFlightSeatRequestDtoValidator()
        {
            RuleFor(x => x.flightId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.source).NotEmpty()
             .Must(dir => dir.Equals("Delta", StringComparison.OrdinalIgnoreCase) || dir.Equals("Southwest", StringComparison.OrdinalIgnoreCase))
         .WithMessage("source must be either 'Delta' or 'Southwest'.");
            RuleFor(x => x.direction)
                .NotEmpty()
                .Must(dir => dir.Equals("outbound", StringComparison.OrdinalIgnoreCase) || dir.Equals("return", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Direction must be either 'outbound' or 'return'.");
        }
    }

    public class GetconnectingFlightSeatRequestDtoValidator : AbstractValidator<GetconnectingFlightSeatRequestDto>
    {
        public GetconnectingFlightSeatRequestDtoValidator()
        {
            RuleFor(x => x.flight1Id).GreaterThan(0);
            RuleFor(x => x.flight2Id).GreaterThan(0);
            RuleFor(x => x.source1).NotEmpty()
             .Must(dir => dir.Equals("Delta", StringComparison.OrdinalIgnoreCase) || dir.Equals("Southwest", StringComparison.OrdinalIgnoreCase))
         .WithMessage("source must be either 'Delta' or 'Southwest'.");
            RuleFor(x => x.source2).NotEmpty()
            .Must(dir => dir.Equals("Delta", StringComparison.OrdinalIgnoreCase) || dir.Equals("Southwest", StringComparison.OrdinalIgnoreCase))
         .WithMessage("source must be either 'Delta' or 'Southwest'.");
            RuleFor(x => x.direction)
                .NotEmpty()
                .Must(dir => dir.Equals("outbound", StringComparison.OrdinalIgnoreCase) || dir.Equals("return", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Direction must be either 'outbound' or 'return'.");
        }
    }
}
