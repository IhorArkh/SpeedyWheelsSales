using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Interfaces;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Ad.Commands.MarkAdAsSold;

public class MarkAdAsSoldCommandHandler : IRequestHandler<MarkAdAsSoldCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public MarkAdAsSoldCommandHandler(DataContext context, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<Unit>> Handle(MarkAdAsSoldCommand request, CancellationToken cancellationToken)
    {
        var ad = await _context.Ads
            .Include(x => x.AppUser)
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (ad is null)
            return Result<Unit>.Empty();

        var currUsername = _currentUserAccessor.GetCurrentUsername();

        if (ad.AppUser.UserName != currUsername)
            return Result<Unit>.Failure("Users can mark as sold only their own ads.");

        ad.IsSold = true;
        ad.SoldAt = DateTime.UtcNow;

        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
            return Result<Unit>.Failure("Failed to mark ad as sold.");

        return Result<Unit>.Success(Unit.Value);
    }
}