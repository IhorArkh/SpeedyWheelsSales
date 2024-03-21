using MediatR;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Profile.Queries.GetCurrUserProfileQuery.DTOs;

namespace SpeedyWheelsSales.Application.Features.Profile.Queries.GetCurrUserProfileQuery;

public record GetCurrUserProfileQuery : IRequest<Result<CurrUserProfileDto>>
{
}