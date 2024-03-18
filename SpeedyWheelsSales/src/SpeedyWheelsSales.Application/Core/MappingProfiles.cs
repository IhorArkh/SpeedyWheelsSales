﻿using AutoMapper;
using Domain.Entities;
using SpeedyWheelsSales.Application.Ad.Commands.CreateAd.DTOs;
using SpeedyWheelsSales.Application.Ad.Commands.UpdateAd.DTOs;
using SpeedyWheelsSales.Application.Ad.Queries.GetAdDetails.DTOs;
using SpeedyWheelsSales.Application.Ad.Queries.GetAdList.DTOs;


namespace SpeedyWheelsSales.Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Car, CarDetailsDto>();
        CreateMap<AppUser, AppUserDetailsDto>();
        CreateMap<Photo, PhotoDetailsDto>();
        CreateMap<Domain.Entities.Ad, AdDetailsDto>()
            .ForMember(dest => dest.CarDetailsDto, opt =>
                opt.MapFrom(src => src.Car))
            .ForMember(dest => dest.AppUserDetailsDto, opt =>
                opt.MapFrom(src => src.AppUser))
            .ForMember(dest => dest.PhotoDetailsDtos, opt =>
                opt.MapFrom(src => src.Photo));

        CreateMap<Domain.Entities.Ad, AdDto>()
            .ForMember(dest => dest.CarDetailsDto, opt =>
                opt.MapFrom(src => src.Car))
            .ForMember(dest => dest.PhotoDetailsDtos, opt =>
                opt.MapFrom(src => src.Photo));

        CreateMap<CreateAdCarDto, Car>();
        CreateMap<CreateAdPhotoDto, Photo>();
        CreateMap<CreateAdDto, Domain.Entities.Ad>()
            .ForMember(dest => dest.Car, opt =>
                opt.MapFrom(src => src.CreateAdCarDto))
            .ForMember(dest => dest.Photo, opt =>
                opt.MapFrom(src => src.CreateAdPhotoDtos));

        CreateMap<UpdateAdCarDto, Car>();
        CreateMap<UpdateAdDto, Domain.Entities.Ad>()
            .ForMember(dest => dest.Car, opt =>
                opt.MapFrom(src => src.UpdateAdCarDto));

        CreateMap<Car, CarListDto>();
        CreateMap<Domain.Entities.Ad, AdDto>()
            .ForMember(dest => dest.CarDetailsDto, opt =>
                opt.MapFrom(src => src.Car))
            .ForMember(dest => dest.PhotoDetailsDtos, opt =>
                opt.MapFrom(src => src.Photo));
    }
}