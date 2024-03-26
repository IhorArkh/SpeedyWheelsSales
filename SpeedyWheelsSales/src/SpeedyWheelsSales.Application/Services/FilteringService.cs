using System.Linq.Expressions;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;
using SpeedyWheelsSales.Application.Interfaces;

namespace SpeedyWheelsSales.Application.Services;

public class FilteringService : IFilteringService
{
    public IQueryable<AdListDto> FilterAds(IQueryable<AdListDto> query, AdParams adParams)
    {
        if (!string.IsNullOrWhiteSpace(adParams.Model))
            query = query.Where(x => x.CarDto.Model.Contains(adParams.Model));

        if (adParams.MinPrice != default || adParams.MaxPrice != default)
        {
            adParams.MinPrice ??= 0;
            adParams.MaxPrice ??= decimal.MaxValue;

            query = query.Where(x => x.CarDto.Price >= adParams.MinPrice && x.CarDto.Price <= adParams.MaxPrice);
        }

        return query;
    }
}