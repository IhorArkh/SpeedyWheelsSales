using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Ad.Commands.DeleteAd;

public class DeleteAdCommandHandler : IRequestHandler<DeleteAdCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public DeleteAdCommandHandler(DataContext context, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<Unit>> Handle(DeleteAdCommand request, CancellationToken cancellationToken)
    {
        var ad = await _context.Ads
            .Include(x => x.AppUser)
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (ad is null)
            return Result<Unit>.Empty();

        var currUsername = _currentUserAccessor.GetCurrentUsername();
        
        if (ad.AppUser.UserName != currUsername)
            return Result<Unit>.Failure("Users can delete only their own ads.");

        _context.Remove(ad);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
            return Result<Unit>.Failure("Failed to delete ad.");

        return Result<Unit>.Success(Unit.Value);
    }
}