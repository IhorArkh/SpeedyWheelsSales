using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Interfaces;
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
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public GetAdListQueryHandler(
        DataContext context,
        IMapper mapper,
        IFilteringService filteringService,
        ISortingService sortingService,
        ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _mapper = mapper;
        _filteringService = filteringService;
        _sortingService = sortingService;
        _currentUserAccessor = currentUserAccessor;
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

        var currUsername = _currentUserAccessor.GetCurrentUsername();

        if (currUsername != null)
            adsQuery = await FillIsFavouriteAndIsAuthorProperties(adsQuery, currUsername); // TODO add tests for this case.

        return Result<PagedList<AdListDto>>.Success(
            await PagedList<AdListDto>.CreateAsync(adsQuery, request.AdParams.PageNumber,
                request.AdParams.PageSize)
        );
    }

    private async Task<IQueryable<AdListDto>> FillIsFavouriteAndIsAuthorProperties
        (IQueryable<AdListDto> query, string currUsername)
    {
        var user = await _context.AppUsers
            .Include(x => x.FavouriteAds)
            .FirstOrDefaultAsync(x => x.UserName == currUsername);

        if (user is null)
            return query;

        var favAdIds = user.FavouriteAds.Select(x => x.AdId).ToList();

        return query.Select(ad => new AdListDto
        {
            Id = ad.Id,
            AppUserId = ad.AppUserId,
            City = ad.City,
            CreatedAt = ad.CreatedAt,
            IsDeleted = ad.IsDeleted,
            IsSold = ad.IsSold,
            SoldAt = ad.SoldAt,
            CarDto = ad.CarDto,
            PhotoDtos = ad.PhotoDtos,
            IsFavourite = favAdIds.Contains(ad.Id),
            IsAuthor = ad.AppUserId == user.Id
        });
    }
}