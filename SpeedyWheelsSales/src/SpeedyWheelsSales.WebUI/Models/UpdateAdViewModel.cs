using SpeedyWheelsSales.Application.Features.Ad.Commands.UpdateAd.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdDetails.DTOs;

namespace SpeedyWheelsSales.WebUI.Models;

public class UpdateAdViewModel
{
    public int AdId { get; set; }
    public UpdateAdDto UpdateAdDto { get; set; }
    public ICollection<AdDetailsPhotoDto> PhotoDtos { get; set; } = new List<AdDetailsPhotoDto>();
}