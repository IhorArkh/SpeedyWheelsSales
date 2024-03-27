using MediatR;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Features.Ad.Commands.ToggleFavouriteAd;

public record ToggleFavouriteAdCommand : IRequest<Result<Unit>>
{
    public int AdId { get; set; }
}