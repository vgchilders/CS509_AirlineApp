using FluentValidation;

namespace AppBackend.DTOs.Validators
{
    public class CreateStripeSessionRequestDtoValidator : AbstractValidator<CreateStripeSessionRequestDto>
    {
        public CreateStripeSessionRequestDtoValidator()
        {
            RuleFor(x => x.SessionId)
                .NotEmpty().WithMessage("SessionId is required.")
                .Must(BeValidGuid).WithMessage("SessionId must be a valid GUID.");

            RuleFor(x => x.BookingReference)
                .NotEmpty().WithMessage("BookingReference is required.")
                .MaximumLength(100).WithMessage("BookingReference must not exceed 100 characters.");
        }

        private bool BeValidGuid(string sessionId)
        {
            return Guid.TryParse(sessionId, out _);
        }
    }
}
