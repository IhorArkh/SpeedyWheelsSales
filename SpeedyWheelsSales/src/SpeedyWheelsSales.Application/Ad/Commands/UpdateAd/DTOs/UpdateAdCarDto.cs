using Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SpeedyWheelsSales.Application.Ad.Commands.UpdateAd.DTOs;

public class UpdateAdCarDto
{
    public decimal Price { get; set; }
    public int Mileage { get; set; }
    public decimal EngineSize { get; set; }
    public double FuelConsumption { get; set; }
    public EngineType EngineType { get; set; }
    public Transmission Transmission { get; set; }
    public TypeOfDrive TypeOfDrive { get; set; }
    public Colors Color { get; set; }
}