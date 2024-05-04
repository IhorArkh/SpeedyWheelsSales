using System.Linq.Expressions;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;
using SpeedyWheelsSales.Application.Interfaces;

namespace SpeedyWheelsSales.Application.Services;

public class SortingService : ISortingService
{
    public IQueryable<AdListDto> SortAds(IQueryable<AdListDto> query, AdParams adParams)
    {
        if (adParams.SortOrder?.ToLower() == "asc")
        {
            query = query.OrderBy(GetSortProperty(adParams));
        }
        else
        {
            query = query.OrderByDescending(GetSortProperty(adParams));
        }

        return query;
    }

    private static Expression<Func<AdListDto, object>> GetSortProperty(AdParams adParams)
    {
        return adParams.SortColumn?.ToLower() switch
        {
            "price" => ad => ad.CarDto.Price,
            "mileage" => ad => ad.CarDto.Mileage,
            "createdat" => ad => ad.CreatedAt,
            "manufacturedate" => ad => ad.CarDto.ManufactureDate,
            _ => ad => ad.CreatedAt
        };
    }
}