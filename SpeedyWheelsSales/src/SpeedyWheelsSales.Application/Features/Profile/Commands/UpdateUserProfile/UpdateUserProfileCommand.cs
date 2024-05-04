using MediatR;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Profile.Commands.UpdateUserProfile.DTOs;

namespace SpeedyWheelsSales.Application.Features.Profile.Commands.UpdateUserProfile;

public record UpdateUserProfileCommand : IRequest<Result<Unit>>
{
    public UpdateUserProfileDto UpdateUserProfileDto { get; set; }
}