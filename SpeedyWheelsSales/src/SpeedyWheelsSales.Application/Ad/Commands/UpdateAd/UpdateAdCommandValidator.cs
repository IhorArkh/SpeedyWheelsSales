using FluentValidation;
using SpeedyWheelsSales.Application.Ad.Commands.UpdateAd.DTOs;

namespace SpeedyWheelsSales.Application.Ad.Commands.UpdateAd;

public class UpdateAdCommandValidator : AbstractValidator<UpdateAdDto>
{
    public UpdateAdCommandValidator()
    {
        //UpdateAdDto
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(2000)
            .WithMessage("Description must be less than 2000 characters.");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required.")
            .MaximumLength(50)
            .WithMessage("City must be less than 50 characters.");

        //UpdateAdCarDto
        RuleFor(x => x.UpdateAdCarDto.Price)
            .NotEmpty()
            .WithMessage("Price is required.")
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");

        RuleFor(x => x.UpdateAdCarDto.Mileage)
            .NotEmpty()
            .WithMessage("Mileage is required.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Mileage must be greater than or equal 0.");

        RuleFor(x => x.UpdateAdCarDto.EngineSize)
            .NotEmpty()
            .WithMessage("EngineSize is required.")
            .GreaterThan(0)
            .WithMessage("EngineSize must be greater than 0.");

        RuleFor(x => x.UpdateAdCarDto.FuelConsumption)
            .NotEmpty()
            .WithMessage("FuelConsumption is required.")
            .GreaterThan(0)
            .WithMessage("FuelConsumption must be greater than 0.");

        RuleFor(x => x.UpdateAdCarDto.Vin)
            .MaximumLength(50)
            .WithMessage("Vin must be less than 50 characters.");

        RuleFor(x => x.UpdateAdCarDto.Plates)
            .MaximumLength(15)
            .WithMessage("Plates must be less than 15 characters.");

        // RuleFor(x => x.UpdateAdCarDto.EngineType)
        //     .NotEmpty()
        //     .WithMessage("EngineType is required.");
        //
        // RuleFor(x => x.UpdateAdCarDto.Transmission)
        //     .NotEmpty()
        //     .WithMessage("Transmission is required.");
        //
        // RuleFor(x => x.UpdateAdCarDto.TypeOfDrive)
        //     .NotEmpty()
        //     .WithMessage("TypeOfDrive is required.");
        //
        // RuleFor(x => x.UpdateAdCarDto.Color)
        //     .NotEmpty()
        //     .WithMessage("Color is required.");
    }
}