using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetFavouriteAds.DTOs;
using SpeedyWheelsSales.WebUI.Core;

namespace SpeedyWheelsSales.WebUI.Models;

public class FavoriteAdsViewModel : WebUiPaginationParams
{
    public List<FavouriteAdDto> FavouriteAdDtos { get; set; } = new List<FavouriteAdDto>();
    public PagingParams PagingParams { get; set; }
    
}