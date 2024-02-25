using Domain.Enums;
using DriveType = Domain.Enums.DriveType;

namespace Domain;

public class Car
{
    public int Id { get; set; }
    public string Model { get; set; }
    public int Price { get; set; }
    public int Mileage { get; set; }
    public double EngineSize { get; set; }
    public double FuelConsumption { get; set; }
    public DateTime ManufactureDate { get; set; }
    public EngineType EngineType { get; set; }
    public Transmission Transmission { get; set; }
    public DriveType DriveType { get; set; }
    public Colors Color { get; set; }

    public int AdId { get; set; }
    
    public Ad Ad { get; set; }
}