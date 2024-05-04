using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;
using SpeedyWheelsSales.WebUI.Core;

namespace SpeedyWheelsSales.WebUI.Models;

public class AdListViewModel : WebUiPaginationParams
{
    public List<AdListDto> Ads { get; set; }
    public AdParams AdParams { get; set; }
}