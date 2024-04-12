using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Interfaces;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Ad.Commands.AddAdPhoto;

public class AddAdPhotoCommandHandler : IRequestHandler<AddAdPhotoCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IPhotoAccessor _photoAccessor;

    public AddAdPhotoCommandHandler(
        DataContext context,
        ICurrentUserAccessor currentUserAccessor,
        IPhotoAccessor photoAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
        _photoAccessor = photoAccessor;
    }

    // TODO add tests for this method
    public async Task<Result<Unit>> Handle(AddAdPhotoCommand request, CancellationToken cancellationToken)
    {
        var ad = await _context.Ads
            .Include(x => x.AppUser)
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.Id == request.AdId);

        if (ad is null)
            return Result<Unit>.Empty();

        var currUsername = _currentUserAccessor.GetCurrentUsername();
        if (currUsername is null)
            return Result<Unit>.Empty();

        if (ad.AppUser.UserName != currUsername)
            return Result<Unit>.Failure("You can add photos only for your ads.");

        var photoUploadResult = await _photoAccessor.AddPhoto(request.Photo);
        if (photoUploadResult is null)
            return Result<Unit>.Failure("Error adding photo.");

        var photo = new Photo
        {
            Url = photoUploadResult.Url,
            Id = photoUploadResult.PublicId
        };

        if (!ad.Photos.Any(x => x.IsMain))
            photo.IsMain = true;

        ad.Photos.Add(photo);

        var result = await _context.SaveChangesAsync() > 0;
        if (!result)
            return Result<Unit>.Failure("Failed to create ad.");

        return Result<Unit>.Success(Unit.Value);
    }
}