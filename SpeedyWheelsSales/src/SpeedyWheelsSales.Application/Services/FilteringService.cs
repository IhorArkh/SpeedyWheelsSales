using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;
using SpeedyWheelsSales.Application.Interfaces;

namespace SpeedyWheelsSales.Application.Services;

public class FilteringService : IFilteringService
{
    public IQueryable<AdListDto> FilterAds(IQueryable<AdListDto> query, AdParams adParams)
    {
        if (!string.IsNullOrWhiteSpace(adParams.City))
            query = query.Where(x => x.City == adParams.City);

        if (!string.IsNullOrWhiteSpace(adParams.Model))
            query = query.Where(x => x.CarDto.Model.Contains(adParams.Model));

        if (adParams.MinPrice != default || adParams.MaxPrice != default)
        {
            adParams.MinPrice ??= 0;
            adParams.MaxPrice ??= decimal.MaxValue;

            query = query.Where(x => x.CarDto.Price >= adParams.MinPrice && x.CarDto.Price <= adParams.MaxPrice);
        }

        if (adParams.MaxMileage != default)
            query = query.Where(x => x.CarDto.Mileage <= adParams.MaxMileage);

        if (adParams.MinEngineSize != default || adParams.MaxEngineSize != default)
        {
            adParams.MinEngineSize ??= 0;
            adParams.MaxEngineSize ??= double.MaxValue;

            query = query.Where(x =>
                x.CarDto.EngineSize >= adParams.MinEngineSize && x.CarDto.EngineSize <= adParams.MaxEngineSize);
        }

        if (adParams.MaxFuelConsumption != default)
            query = query.Where(x => x.CarDto.FuelConsumption <= adParams.MaxFuelConsumption);

        if (adParams.MinManufactureYear != default || adParams.MaxManufactureYear != default)
        {
            adParams.MinManufactureYear ??= 0;
            adParams.MaxManufactureYear ??= int.MaxValue;

            query = query.Where(x => x.CarDto.ManufactureDate.Year >= adParams.MinManufactureYear &&
                                     x.CarDto.ManufactureDate.Year <= adParams.MaxManufactureYear);
        }

        if (adParams.EngineType != default)
            query = query.Where(x => x.CarDto.EngineType == adParams.EngineType);

        if (adParams.Transmission != default)
            query = query.Where(x => x.CarDto.Transmission == adParams.Transmission);

        if (adParams.TypeOfDrive != default)
            query = query.Where(x => x.CarDto.TypeOfDrive == adParams.TypeOfDrive);

        if (adParams.Color != default)
            query = query.Where(x => x.CarDto.Color == adParams.Color);

        return query;
    }
}