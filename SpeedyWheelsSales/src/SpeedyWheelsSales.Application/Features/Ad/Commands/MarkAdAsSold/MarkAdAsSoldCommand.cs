using MediatR;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Features.Ad.Commands.MarkAdAsSold;

public record MarkAdAsSoldCommand : IRequest<Result<Unit>>
{
    public int Id { get; set; }
}