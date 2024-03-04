using MediatR;
using SpeedyWheelsSales.Application.Ad.Commands.UpdateAd.DTOs;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Ad.Commands.UpdateAd;

public class UpdateAdCommand : IRequest<Result<Unit>>
{
    public int Id { get; set; }
    public UpdateAdDto UpdateAdDto { get; set; }
}