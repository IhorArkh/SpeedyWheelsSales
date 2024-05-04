using MediatR;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Profile.Queries.GetUserProfile.DTOs;

namespace SpeedyWheelsSales.Application.Features.Profile.Queries.GetUserProfile;

public record GetUserProfileQuery : IRequest<Result<UserProfileDto>>
{
    public string? Username { get; set; }
}