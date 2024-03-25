using MediatR;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;

namespace SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList;

public record GetAdListQuery : IRequest<Result<PagedList<AdListDto>>>
{
    public PagingParams PagingParams { get; set; }
}