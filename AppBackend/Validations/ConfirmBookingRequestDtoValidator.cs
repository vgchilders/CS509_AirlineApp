using FluentValidation;

namespace AppBackend.DTOs.Validators
{
    public class ConfirmBookingRequestDtoValidator : AbstractValidator<ConfirmBookingRequestDto>
    {
        public ConfirmBookingRequestDtoValidator()
        {
            RuleFor(x => x.ConfirmationCode)
                .NotEmpty().WithMessage("Confirmation code is required.")
                .MaximumLength(100).WithMessage("Confirmation code must not exceed 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.")
                .Matches(@"^[A-Za-z]").WithMessage("Last name start With a latter.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
