using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;

namespace SpeedyWheelsSales.WebUI.Models;

public class AdListViewModel
{
    public List<AdListDto> Ads { get; set; }
    public AdParams AdParams { get; set; }
}