using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Features.Ad.Commands.DeleteAdPhoto;

public record DeleteAdPhotoCommand : IRequest<Result<Unit>>
{
    public string PhotoId { get; set; }
}