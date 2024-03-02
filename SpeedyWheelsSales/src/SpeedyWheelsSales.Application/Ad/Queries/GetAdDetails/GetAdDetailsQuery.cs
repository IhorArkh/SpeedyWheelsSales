using MediatR;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Ad.Queries.GetAdDetails;

public class GetAdDetailsQuery : IRequest<Result<Domain.Ad>>
{
    public int Id { get; set; }
}