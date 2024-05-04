using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Interfaces;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Profile.Commands.ChangeProfilePhoto;

public class ChangeProfilePhotoCommandHandler : IRequestHandler<ChangeProfilePhotoCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IPhotoAccessor _photoAccessor;

    public ChangeProfilePhotoCommandHandler(
        DataContext context,
        ICurrentUserAccessor currentUserAccessor,
        IPhotoAccessor photoAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
        _photoAccessor = photoAccessor;
    }

    //TODO add tests for this method
    public async Task<Result<Unit>> Handle(ChangeProfilePhotoCommand request, CancellationToken cancellationToken)
    {
        var currUsername = _currentUserAccessor.GetCurrentUsername();
        if (currUsername is null)
            return Result<Unit>.Empty();

        var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserName == currUsername);
        if (user is null)
            return Result<Unit>.Empty();

        if (user.PhotoPublicId != null)
        {
            var deletePhotoResult = await _photoAccessor.DeletePhoto(user.PhotoPublicId);
            if (deletePhotoResult is null)
                return Result<Unit>.Failure("Error deleting previous photo.");
        }

        var photoUploadResult = await _photoAccessor.AddPhoto(request.Photo);
        if (photoUploadResult is null)
            return Result<Unit>.Failure("Error adding new photo.");

        user.PhotoPublicId = photoUploadResult.PublicId;
        user.PhotoUrl = photoUploadResult.Url;

        var result = await _context.SaveChangesAsync() > 0;
        if (!result)
            return Result<Unit>.Failure("Failed to change photo");

        return Result<Unit>.Success(Unit.Value);
    }
};