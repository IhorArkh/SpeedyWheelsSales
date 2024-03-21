using AutoMapper;
using Domain.Entities;
using SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Commands.UpdateAd.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdDetails.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;
using SpeedyWheelsSales.Application.Features.Profile.Queries.GetCurrUserProfileQuery.DTOs;


namespace SpeedyWheelsSales.Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // GetAdDetails
        CreateMap<Domain.Entities.Ad, AdDetailsDto>()
            .ForMember(dest => dest.CarDto, opt =>
                opt.MapFrom(src => src.Car))
            .ForMember(dest => dest.AppUserDtos, opt =>
                opt.MapFrom(src => src.AppUser))
            .ForMember(dest => dest.PhotoDtos, opt =>
                opt.MapFrom(src => src.Photo));
        CreateMap<Car, AdDetailsCarDto>();
        CreateMap<AppUser, AdDetailsAppUserDto>();
        CreateMap<Photo, AdDetailsPhotoDto>();

        // GetAdList
        CreateMap<Domain.Entities.Ad, AdListDto>()
            .ForMember(dest => dest.CarDto, opt =>
                opt.MapFrom(src => src.Car))
            .ForMember(dest => dest.PhotoDtos, opt =>
                opt.MapFrom(src => src.Photo));
        CreateMap<Photo, AdListPhotoDto>();
        CreateMap<Car, AdListCarDto>();

        // CreateAd
        CreateMap<CreateAdDto, Domain.Entities.Ad>()
            .ForMember(dest => dest.Car, opt =>
                opt.MapFrom(src => src.CreateAdCarDto))
            .ForMember(dest => dest.Photo, opt =>
                opt.MapFrom(src => src.CreateAdPhotoDtos));
        CreateMap<CreateAdCarDto, Car>();
        CreateMap<CreateAdPhotoDto, Photo>();

        // UpdateAd
        CreateMap<UpdateAdDto, Domain.Entities.Ad>()
            .ForMember(dest => dest.Car, opt =>
                opt.MapFrom(src => src.UpdateAdCarDto));
        CreateMap<UpdateAdCarDto, Car>();

        // GetCurrUserProfile
        CreateMap<Ad, CurrUserAdDto>()
            .ForMember(dest => dest.CarDto, opt =>
                opt.MapFrom(src => src.Car))
            .ForMember(dest => dest.PhotoDtos, opt =>
                opt.MapFrom(src => src.Photo));
        CreateMap<AppUser, CurrUserProfileDto>()
            .ForMember(dest => dest.AdDtos, opt =>
                opt.MapFrom(src => src.Ads));
        CreateMap<Car, CurrUserCarDto>();
        CreateMap<Photo, CurrUserPhotoDto>();
    }
}