using System.ComponentModel;
using System.Security.AccessControl;
using Domain.Enums;
using DriveType = Domain.Enums.DriveType;

namespace Domain;

public class Ad
{
    public Guid Id { get; set; }
    public string Model { get; set; }
    public int Price { get; set; }
    public int Mileage { get; set; }
    public double EngineSize { get; set; }
    public DateOnly ManufactureDate { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsSold { get; set; }
    public DateTime SoldAt { get; set; }
    public double FuelConsumption { get; set; }
    public EngineType EngineType { get; set; }
    public Transmission Transmission { get; set; }
    public DriveType DriveType { get; set; }
    public Colors Color { get; set; }
    public AppUser AppUser { get; set; }
}