using MediatR;
using SpeedyWheelsSales.Application.Ad.Commands.CreateAd.DTOs;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Ad.Commands.CreateAd;

public class CreateAdCommand : IRequest<Result<Unit>>
{
    public CreateAdDto CreateAdDto { get; set; }
}