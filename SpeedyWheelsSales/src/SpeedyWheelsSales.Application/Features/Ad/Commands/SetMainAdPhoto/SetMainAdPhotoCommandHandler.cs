using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Interfaces;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Ad.Commands.SetMainAdPhoto;

public class SetMainAdPhotoCommandHandler : IRequestHandler<SetMainAdPhotoCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public SetMainAdPhotoCommandHandler(DataContext context, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    // TODO add tests for this method
    public async Task<Result<Unit>> Handle(SetMainAdPhotoCommand request, CancellationToken cancellationToken)
    {
        var currUsername = _currentUserAccessor.GetCurrentUsername();
        if (currUsername is null)
            return Result<Unit>.Empty();

        var photo = await _context.Photos
            .Include(x => x.Ad)
            .ThenInclude(x => x.AppUser)
            .Include(x => x.Ad)
            .ThenInclude(x => x.Photos)
            .FirstOrDefaultAsync(x => x.Id == request.PhotoId);

        if (photo is null)
            return Result<Unit>.Empty();

        if (currUsername != photo.Ad.AppUser.UserName)
            return Result<Unit>.Failure("You can set main photo only for your ads.");

        var prevMainPhoto = photo.Ad.Photos.FirstOrDefault(x => x.IsMain);
        if (prevMainPhoto != null)
            prevMainPhoto.IsMain = false;

        photo.IsMain = true;

        var result = await _context.SaveChangesAsync() > 0;
        if (!result)
            return Result<Unit>.Failure("Failed to set photo as main.");

        return Result<Unit>.Success(Unit.Value);
    }
}