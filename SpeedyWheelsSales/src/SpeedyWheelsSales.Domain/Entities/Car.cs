using Domain.Enums;

namespace Domain;

public class Car
{
    public int Id { get; set; }
    public string Model { get; set; }
    public decimal Price { get; set; }
    public int Mileage { get; set; }
    public double EngineSize { get; set; }
    public double FuelConsumption { get; set; }
    public string Vin { get; set; }
    public string Plates { get; set; }
    public DateTime ManufactureDate { get; set; }
    public EngineType EngineType { get; set; }
    public Transmission Transmission { get; set; }
    public TypeOfDrive TypeOfDrive { get; set; }
    public Colors Color { get; set; }
    public int AdId { get; set; }
    
    public Ad Ad { get; set; }
}