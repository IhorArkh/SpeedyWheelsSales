using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetFavouriteAds.DTOs;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Ad.Queries.GetFavouriteAds;

public class GetFavouriteAdsQueryHandler : IRequestHandler<GetFavouriteAdsQuery, Result<PagedList<FavouriteAdDto>>>
{
    private readonly DataContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IMapper _mapper;

    public GetFavouriteAdsQueryHandler(DataContext context, ICurrentUserAccessor currentUserAccessor,
        IMapper mapper)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<FavouriteAdDto>>> Handle(GetFavouriteAdsQuery request,
        CancellationToken cancellationToken)
    {
        var currUserUsername = _currentUserAccessor.GetCurrentUsername();

        var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserName == currUserUsername);
        if (user is null)
            return Result<PagedList<FavouriteAdDto>>.Empty();

        var favAdsQuery = _context.FavouriteAds
            .Where(x => x.AppUserId == user.Id)
            .Include(x => x.Ad)
            .ThenInclude(x => x.Car)
            .Include(x => x.Ad)
            .ThenInclude(x => x.Photos)
            .ProjectTo<FavouriteAdDto>(_mapper.ConfigurationProvider)
            .AsQueryable();

        return Result<PagedList<FavouriteAdDto>>.Success(
            await PagedList<FavouriteAdDto>.CreateAsync(favAdsQuery, request.PagingParams.PageNumber,
                request.PagingParams.PageSize)
        );
    }
}