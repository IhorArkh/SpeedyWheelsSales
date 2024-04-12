using MediatR;
using Microsoft.AspNetCore.Http;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Features.Ad.Commands.AddAdPhoto;

public record AddAdPhotoCommand : IRequest<Result<Unit>>
{
    public int AdId { get; set; }
    public IFormFile Photo { get; set; }
}