namespace SpeedyWheelsSales.Application.Ad.Commands.UpdateAd.DTOs;

public class UpdateAdDto
{
    public string Description { get; set; }
    public string City { get; set; }
    public bool IsSold { get; set; }
    public UpdateAdCarDto UpdateAdCarDto { get; set; }
}