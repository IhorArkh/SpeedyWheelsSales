using Domain.Enums;

namespace SpeedyWheelsSales.Application.Features.SavedSearch.SaveSearch;

public class SaveSearchParams
{
    // Ad params
    public string? City { get; set; }

    // Car params
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MaxMileage { get; set; }
    public int? MinManufactureYear { get; set; }
    public int? MaxManufactureYear { get; set; }
    public EngineType? EngineType { get; set; }
    public Transmission? Transmission { get; set; }
    public TypeOfDrive? TypeOfDrive { get; set; }
    public Colors? Color { get; set; }
}