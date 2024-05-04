using MediatR;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd.DTOs;

namespace SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd;

public record CreateAdCommand : IRequest<Result<Unit>>
{
    public CreateAdDto CreateAdDto { get; set; }
}