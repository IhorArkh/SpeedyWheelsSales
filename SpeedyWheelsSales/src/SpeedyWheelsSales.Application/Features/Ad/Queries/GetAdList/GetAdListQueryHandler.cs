using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;
using SpeedyWheelsSales.Application.Interfaces;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList;

public class GetAdListQueryHandler : IRequestHandler<GetAdListQuery, Result<PagedList<AdListDto>>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IFilteringService _filteringService;
    private readonly ISortingService _sortingService;

    public GetAdListQueryHandler(DataContext context, IMapper mapper, IFilteringService filteringService,
        ISortingService sortingService)
    {
        _context = context;
        _mapper = mapper;
        _filteringService = filteringService;
        _sortingService = sortingService;
    }

    public async Task<Result<PagedList<AdListDto>>> Handle(GetAdListQuery request, CancellationToken cancellationToken)
    {
        var adsQuery = _context.Ads
            .Include(x => x.Car)
            .Include(x => x.Photos)
            .ProjectTo<AdListDto>(_mapper.ConfigurationProvider)
            .AsQueryable();

        adsQuery = _sortingService.SortAds(adsQuery, request.AdParams);
        adsQuery = _filteringService.FilterAds(adsQuery, request.AdParams);

        return Result<PagedList<AdListDto>>.Success(
            await PagedList<AdListDto>.CreateAsync(adsQuery, request.AdParams.PageNumber,
                request.AdParams.PageSize)
        );
    }
}