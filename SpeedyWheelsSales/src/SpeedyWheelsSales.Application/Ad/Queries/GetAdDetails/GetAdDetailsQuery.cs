using MediatR;

namespace SpeedyWheelsSales.Application.Ad.Queries.GetAdDetails;

public class GetAdDetailsQuery : IRequest<Domain.Ad>
{
    public int Id { get; set; }
}