using MediatR;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Ad.Queries.GetAdList;

public class GetAdListQuery : IRequest<Result<List<Domain.Ad>>>
{
}