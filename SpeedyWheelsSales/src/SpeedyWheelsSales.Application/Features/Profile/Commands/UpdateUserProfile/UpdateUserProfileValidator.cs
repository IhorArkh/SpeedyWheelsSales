using FluentValidation;
using SpeedyWheelsSales.Application.Features.Profile.Commands.UpdateUserProfile.DTOs;

namespace SpeedyWheelsSales.Application.Features.Profile.Commands.UpdateUserProfile;

public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileDto>
{
    public UpdateUserProfileValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(50)
            .WithMessage("Name must be less than 50 characters.");

        RuleFor(x => x.Location)
            .NotEmpty()
            .WithMessage("Location is required.")
            .MaximumLength(50)
            .WithMessage("Location must be less than 50 characters.");

        RuleFor(x => x.Bio)
            .MaximumLength(250)
            .WithMessage("Bio must be less than 250 characters.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .MaximumLength(50)
            .WithMessage("Email must be less than 50 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("PhoneNumber is required.")
            .MaximumLength(20)
            .WithMessage("PhoneNumber must be less than 20 characters.");
    }
}