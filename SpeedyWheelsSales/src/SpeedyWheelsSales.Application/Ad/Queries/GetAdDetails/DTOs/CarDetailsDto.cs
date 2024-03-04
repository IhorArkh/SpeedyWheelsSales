using Domain.Enums;
using DriveType = System.IO.DriveType;

namespace SpeedyWheelsSales.Application.Ad.Queries.GetAdDetails.DTOs;

public class CarDetailsDto
{
    public int Id { get; set; }
    public string Model { get; set; }
    public decimal Price { get; set; }
    public int Mileage { get; set; }
    public decimal EngineSize { get; set; }
    public double FuelConsumption { get; set; }
    public DateTime ManufactureDate { get; set; }
    public EngineType EngineType { get; set; }
    public Transmission Transmission { get; set; }
    public DriveType DriveType { get; set; }
    public Colors Color { get; set; }
}