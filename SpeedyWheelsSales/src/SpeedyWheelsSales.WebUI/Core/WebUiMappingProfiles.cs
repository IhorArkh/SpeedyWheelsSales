using AutoMapper;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Commands.UpdateAd.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdDetails.DTOs;
using SpeedyWheelsSales.Application.Features.SavedSearch.Commands.SaveSearch;

namespace SpeedyWheelsSales.WebUI.Core;

public class WebUiMappingProfiles : Profile
{
    public WebUiMappingProfiles()
    {
        CreateMap<AdParams, SaveSearchParams>();

        CreateMap<AdDetailsDto, UpdateAdDto>()
            .ForMember(dest => dest.UpdateAdCarDto, opt =>
                opt.MapFrom(x => x.CarDto));
        CreateMap<AdDetailsCarDto, UpdateAdCarDto>();
    }
}