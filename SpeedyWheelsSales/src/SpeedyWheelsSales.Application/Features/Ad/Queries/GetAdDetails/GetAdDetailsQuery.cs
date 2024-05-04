using MediatR;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdDetails.DTOs;

namespace SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdDetails;

public record GetAdDetailsQuery : IRequest<Result<AdDetailsDto>>
{
    public int Id { get; set; }
}