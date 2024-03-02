using MediatR;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Ad.Commands.CreateAd;

public class CreateAdCommand : IRequest<Result<Unit>>
{
    public Domain.Ad Ad { get; set; }
}