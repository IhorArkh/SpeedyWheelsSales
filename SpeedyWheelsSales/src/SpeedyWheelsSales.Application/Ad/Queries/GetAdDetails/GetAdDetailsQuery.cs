using MediatR;
using SpeedyWheelsSales.Application.Ad.DTOs;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Ad.Queries.GetAdDetails;

public class GetAdDetailsQuery : IRequest<Result<AdDetailsDto>>
{
    public int Id { get; set; }
}