using MediatR;
using Microsoft.AspNetCore.Http;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Features.Profile.Commands.ChangeProfilePhoto;

public record ChangeProfilePhotoCommand : IRequest<Result<Unit>>
{
    public IFormFile Photo { get; set; }
}