using SpeedyWheelsSales.Application.Features.Ad.Commands.UpdateAd.DTOs;

namespace SpeedyWheelsSales.WebUI.Models;

public class UpdateAdViewModel
{
    public int AdId { get; set; }
    public UpdateAdDto UpdateAdDto { get; set; }
}