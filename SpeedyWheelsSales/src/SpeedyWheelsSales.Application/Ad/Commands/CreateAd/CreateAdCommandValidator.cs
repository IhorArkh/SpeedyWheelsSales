using FluentValidation;
using SpeedyWheelsSales.Application.Ad.Commands.CreateAd.DTOs;

namespace SpeedyWheelsSales.Application.Ad.Commands.CreateAd;

public class CreateAdCommandValidator : AbstractValidator<CreateAdDto>
{
    public CreateAdCommandValidator()
    {
        //CreateAdDto
        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required.")
            .MaximumLength(50)
            .WithMessage("City must be less than 50 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(2000)
            .WithMessage("Description must be less than 2000 characters.");

        //CreateAdCarDto
        RuleFor(x => x.CreateAdCarDto.Model)
            .NotEmpty()
            .WithMessage("Model is required.")
            .MaximumLength(50)
            .WithMessage("Model must be less than 50 characters.");

        RuleFor(x => x.CreateAdCarDto.Price)
            .NotEmpty()
            .WithMessage("Price is required.")
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");
        
        RuleFor(x => x.CreateAdCarDto.Mileage)
            .NotEmpty()
            .WithMessage("Mileage is required.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Mileage must be greater than or equal 0.");
        
        RuleFor(x => x.CreateAdCarDto.EngineSize)
            .NotEmpty()
            .WithMessage("EngineSize is required.")
            .GreaterThan(0)
            .WithMessage("EngineSize must be greater than 0.");
        
        RuleFor(x => x.CreateAdCarDto.FuelConsumption)
            .NotEmpty()
            .WithMessage("FuelConsumption is required.")
            .GreaterThan(0)
            .WithMessage("FuelConsumption must be greater than 0.");
        
        RuleFor(x => x.CreateAdCarDto.Vin)
            .MaximumLength(50)
            .WithMessage("Vin must be less than 50 characters.");
        
        RuleFor(x => x.CreateAdCarDto.Plates)
            .MaximumLength(15)
            .WithMessage("Plates must be less than 15 characters.");

        RuleFor(x => x.CreateAdCarDto.ManufactureDate)
            .NotEmpty()
            .WithMessage("ManufactureDate is required.");
        
        // RuleFor(x => x.CreateAdCarDto.EngineType)
        //     .IsInEnum()
        //     .WithMessage("EngineType is required.");
        //
        // RuleFor(x => x.CreateAdCarDto.Transmission)
        //     .IsInEnum()
        //     .WithMessage("Transmission is required.");
        //
        // RuleFor(x => x.CreateAdCarDto.TypeOfDrive)
        //     .IsInEnum()
        //     .WithMessage("TypeOfDrive is required.");
        //
        // RuleFor(x => x.CreateAdCarDto.Color)
        //     .IsInEnum()
        //     .WithMessage("Color is required.");
        //TODO create custom enums validator
    }
}