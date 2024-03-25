using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList;

public class GetAdListQueryHandler : IRequestHandler<GetAdListQuery, Result<PagedList<AdListDto>>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetAdListQueryHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<AdListDto>>> Handle(GetAdListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Ads
            .Include(x => x.Car)
            .Include(x => x.Photos)
            .OrderBy(x => x.CreatedAt)
            .ProjectTo<AdListDto>(_mapper.ConfigurationProvider)
            .AsQueryable();

        return Result<PagedList<AdListDto>>.Success(
            await PagedList<AdListDto>.CreateAsync(query, request.PagingParams.PageNumber,
                request.PagingParams.PageSize)
        );
    }
}