using MediatR;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdDetails.DTOs;

namespace SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdDetails;

public class GetAdDetailsQuery : IRequest<Result<AdDetailsDto>>
{
    public int Id { get; set; }
}