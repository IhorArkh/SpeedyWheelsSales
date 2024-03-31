using AutoMapper;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.SavedSearch.Commands.SaveSearch;

namespace SpeedyWheelsSales.WebUI.Core;

public class WebUiMappingProfiles : Profile
{
    public WebUiMappingProfiles()
    {
        CreateMap<AdParams, SaveSearchParams>();
    }
}