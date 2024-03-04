using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Interfaces;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Commands.DeleteAd;

public class DeleteAdCommandHandler : IRequestHandler<DeleteAdCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly IUserAccessor _userAccessor;

    public DeleteAdCommandHandler(DataContext context, IUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor = userAccessor;
    }

    public async Task<Result<Unit>> Handle(DeleteAdCommand request, CancellationToken cancellationToken)
    {
        var ad = await _context.Ads
            .Include(x => x.AppUser)
            .FirstOrDefaultAsync(x => x.Id == request.Id);
        
        if (ad is null)
            return null;

        if (ad.AppUser.UserName != _userAccessor.GetUsername())
            return Result<Unit>.Failure("Users can delete only their own ads.");

        _context.Remove(ad);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
            return Result<Unit>.Failure("Failed to delete ad.");

        return Result<Unit>.Success(Unit.Value);
    }
}