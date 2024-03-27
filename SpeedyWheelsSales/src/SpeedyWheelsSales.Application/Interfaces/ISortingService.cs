using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;

namespace SpeedyWheelsSales.Application.Interfaces;

public interface ISortingService
{
    IQueryable<AdListDto> SortAds(IQueryable<AdListDto> query, AdParams adParams);
}