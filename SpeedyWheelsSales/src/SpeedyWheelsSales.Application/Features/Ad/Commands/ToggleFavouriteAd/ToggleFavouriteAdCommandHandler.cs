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

    public ToggleFavouriteAdCommandHandler(DataContext context, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<Unit>> Handle(ToggleFavouriteAdCommand request, CancellationToken cancellationToken)
    {
        var ad = await _context.Ads
            .FirstOrDefaultAsync(x => x.Id == request.AdId);
        if (ad is null)
            return Result<Unit>.Empty();

        var currUsername = _currentUserAccessor.GetCurrentUsername();

        var user = await _context.AppUsers
            .Include(x => x.FavouriteAds)
            .Include(x => x.Ads)
            .FirstOrDefaultAsync(x => x.UserName == currUsername);
        if (user is null)
            return Result<Unit>.Empty();

        var isUserCreator = user.Ads.FirstOrDefault(x => x.Id == ad.Id);
        if (isUserCreator != null)
            return Result<Unit>.Failure("You can't add your own add to favourites.");

        var existingFavAd = user.FavouriteAds.FirstOrDefault(x => x.AdId == ad.Id);

        if (existingFavAd == null)
        {
            var favAd = new FavouriteAd()
            {
                AdId = ad.Id,
                AppUserId = user.Id,
                Ad = ad
            };

            _context.FavouriteAds.Add(favAd);
        }
        else
        {
            _context.FavouriteAds.Remove(existingFavAd);
        }

        var result = await _context.SaveChangesAsync() > 0;
        if (!result)
            return Result<Unit>.Failure("Failed to toggle favourite ad.");

        return Result<Unit>.Success(Unit.Value);
    }
}