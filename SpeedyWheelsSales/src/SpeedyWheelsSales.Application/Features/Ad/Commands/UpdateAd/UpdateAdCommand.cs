using MediatR;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Commands.UpdateAd.DTOs;

namespace SpeedyWheelsSales.Application.Features.Ad.Commands.UpdateAd;

public class UpdateAdCommand : IRequest<Result<Unit>>
{
    public int Id { get; set; }
    public UpdateAdDto UpdateAdDto { get; set; }
}