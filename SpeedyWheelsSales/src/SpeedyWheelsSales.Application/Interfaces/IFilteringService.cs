using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;

namespace SpeedyWheelsSales.Application.Interfaces;

public interface IFilteringService
{
    IQueryable<AdListDto> FilterAds(IQueryable<AdListDto> query, AdParams adParams);
}