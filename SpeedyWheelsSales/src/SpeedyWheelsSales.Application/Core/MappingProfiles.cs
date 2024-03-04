using AutoMapper;
using Domain;
using Domain.Entities;
using SpeedyWheelsSales.Application.Ad.DTOs;

namespace SpeedyWheelsSales.Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Car, CarDetailsDto>();
        CreateMap<AppUser, AppUserDetailsDto>();
        CreateMap<Photo, PhotoDetailsDto>();
        CreateMap<Domain.Ad, AdDetailsDto>()
            .ForMember(dest => dest.CarDetailsDto, opt =>
                opt.MapFrom(src => src.Car))
            .ForMember(dest => dest.AppUserDetailsDto, opt =>
                opt.MapFrom(src => src.AppUser))
            .ForMember(dest => dest.PhotoDetailsDtos, opt =>
                opt.MapFrom(src => src.Photo));
        
        CreateMap<Domain.Ad, AdDto>()
            .ForMember(dest => dest.CarDetailsDto, opt =>
                opt.MapFrom(src => src.Car))
            .ForMember(dest => dest.PhotoDetailsDtos, opt =>
                opt.MapFrom(src => src.Photo));
    }
}