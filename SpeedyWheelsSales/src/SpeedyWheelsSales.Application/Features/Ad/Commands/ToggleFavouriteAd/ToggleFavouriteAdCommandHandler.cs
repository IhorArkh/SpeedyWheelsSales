using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Ad.Commands.ToggleFavouriteAd;

public class ToggleFavouriteAdCommandHandler : IRequestHandler<ToggleFavouriteAdCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IMapper _mapper;

    public ToggleFavouriteAdCommandHandler(DataContext context, ICurrentUserAccessor currentUserAccessor,
        IMapper mapper)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
        _mapper = mapper;
    }

    public async Task<Result<Unit>> Handle(ToggleFavouriteAdCommand request, CancellationToken cancellationToken)
    {
        var ad = await _context.Ads.FirstOrDefaultAsync(x => x.Id == request.AdId);
        if (ad is null)
            return Result<Unit>.Empty();

        var currUsername = _currentUserAccessor.GetCurrentUsername();

        var user = await _context.AppUsers
            .Include(x => x.FavouriteAds)
            .FirstOrDefaultAsync(x => x.UserName == currUsername);
        if (user is null)
            return Result<Unit>.Empty();

        var existingFavAd = user.FavouriteAds.FirstOrDefault(x => x.Id == ad.Id);

        if (existingFavAd == null)
        {
            var favAd = _mapper.Map<FavouriteAd>(ad);
            user.FavouriteAds.Add(favAd);
        }
        else
        {
            user.FavouriteAds.Remove(existingFavAd);
        }

        var result = await _context.SaveChangesAsync() > 0;
        if (!result)
            return Result<Unit>.Failure("Failed to toggle favourite ad.");

        return Result<Unit>.Success(Unit.Value);
    }
}