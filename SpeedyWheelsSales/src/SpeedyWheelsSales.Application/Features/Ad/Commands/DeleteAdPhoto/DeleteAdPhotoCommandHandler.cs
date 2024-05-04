using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Interfaces;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Ad.Commands.DeleteAdPhoto;

public class DeleteAdPhotoCommandHandler : IRequestHandler<DeleteAdPhotoCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IPhotoAccessor _photoAccessor;

    public DeleteAdPhotoCommandHandler(
        DataContext context,
        ICurrentUserAccessor currentUserAccessor,
        IPhotoAccessor photoAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
        _photoAccessor = photoAccessor;
    }

    //TODO add tests for this method
    public async Task<Result<Unit>> Handle(DeleteAdPhotoCommand request, CancellationToken cancellationToken)
    {
        var currUsername = _currentUserAccessor.GetCurrentUsername();
        if (currUsername is null)
            return Result<Unit>.Empty();

        var photo = await _context.Photos
            .Include(x => x.Ad)
            .ThenInclude(x => x.AppUser)
            .FirstOrDefaultAsync(x => x.Id == request.PhotoId);

        if (photo is null)
            return Result<Unit>.Empty();

        if (currUsername != photo.Ad.AppUser.UserName)
            return Result<Unit>.Failure("You can delete photos only from your ads.");

        if (photo.IsMain)
            return Result<Unit>.Failure("You can't delete main photo.");

        var photoDeletionResult = await _photoAccessor.DeletePhoto(photo.Id);
        if (photoDeletionResult is null)
            return Result<Unit>.Failure("Error deleting photo.");

        _context.Photos.Remove(photo);
        var result = await _context.SaveChangesAsync() > 0;
        if (!result)
            return Result<Unit>.Failure("Failed to delete photo.");

        return Result<Unit>.Success(Unit.Value);
    }
}