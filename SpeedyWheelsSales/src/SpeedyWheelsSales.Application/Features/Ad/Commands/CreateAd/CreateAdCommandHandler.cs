using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd.DTOs;
using SpeedyWheelsSales.Application.Interfaces;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd;

public class CreateAdCommandHandler : IRequestHandler<CreateAdCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IValidator<CreateAdDto> _validator;
    private readonly IPhotoAccessor _photoAccessor;

    public CreateAdCommandHandler(
        DataContext context,
        IMapper mapper,
        ICurrentUserAccessor currentUserAccessor,
        IValidator<CreateAdDto> validator,
        IPhotoAccessor photoAccessor)
    {
        _context = context;
        _mapper = mapper;
        _currentUserAccessor = currentUserAccessor;
        _validator = validator;
        _photoAccessor = photoAccessor;
    }

    public async Task<Result<Unit>> Handle(CreateAdCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request.CreateAdDto);
        if (!validationResult.IsValid)
            return Result<Unit>.ValidationError(validationResult.Errors);

        var currUserUsername = _currentUserAccessor.GetCurrentUsername();
        if (currUserUsername is null)
            return Result<Unit>.Empty();

        var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserName == currUserUsername);
        if (user is null)
            return Result<Unit>.Empty();

        var ad = _mapper.Map<Domain.Entities.Ad>(request.CreateAdDto);
        ad.CreatedAt = DateTime.UtcNow;

        foreach (var file in request.CreateAdDto.Photos)
        {
            var photoUploadResult = await _photoAccessor.AddPhoto(file);
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
        }

        user.Ads.Add(ad);

        var result = await _context.SaveChangesAsync() > 0;
        if (!result)
            return Result<Unit>.Failure("Failed to create ad.");

        return Result<Unit>.Success(Unit.Value);
    }
}