using AutoMapper;

namespace SpeedyWheelsSales.Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Domain.Ad, Domain.Ad>();
    }
}