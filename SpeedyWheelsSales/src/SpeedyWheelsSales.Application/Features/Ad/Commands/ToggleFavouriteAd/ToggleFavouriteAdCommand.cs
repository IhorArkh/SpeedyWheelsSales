using MediatR;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Features.Ad.Commands.ToggleFavouriteAd;

public class ToggleFavouriteAdCommand : IRequest<Result<Unit>>
{
    public int AdId { get; set; }
}