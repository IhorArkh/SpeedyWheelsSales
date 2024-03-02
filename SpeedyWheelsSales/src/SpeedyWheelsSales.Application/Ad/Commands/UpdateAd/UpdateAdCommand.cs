using MediatR;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Ad.Commands.UpdateAd;

public class UpdateAdCommand : IRequest<Result<Unit>>
{
    public Domain.Ad Ad { get; set; }
}