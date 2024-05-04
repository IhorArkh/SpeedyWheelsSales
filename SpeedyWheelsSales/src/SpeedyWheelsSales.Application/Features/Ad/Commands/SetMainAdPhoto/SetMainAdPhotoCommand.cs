using MediatR;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Features.Ad.Commands.SetMainAdPhoto;

public record SetMainAdPhotoCommand : IRequest<Result<Unit>>
{
    public string PhotoId { get; set; }
}