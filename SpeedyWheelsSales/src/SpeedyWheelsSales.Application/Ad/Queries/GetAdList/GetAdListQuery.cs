using MediatR;

namespace SpeedyWheelsSales.Application.Ad.Queries.GetAdList;

public class GetAdListQuery : IRequest<List<Domain.Ad>>
{
}