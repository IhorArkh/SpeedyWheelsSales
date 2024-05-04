using MediatR;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetFavouriteAds.DTOs;

namespace SpeedyWheelsSales.Application.Features.Ad.Queries.GetFavouriteAds;

public record GetFavouriteAdsQuery : IRequest<Result<PagedList<FavouriteAdDto>>>
{
    public PagingParams PagingParams { get; set; }
}