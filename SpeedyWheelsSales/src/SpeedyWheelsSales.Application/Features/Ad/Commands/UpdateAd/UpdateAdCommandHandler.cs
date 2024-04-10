using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Commands.UpdateAd.DTOs;
using SpeedyWheelsSales.Application.Interfaces;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Ad.Commands.UpdateAd;

public class UpdateAdCommandHandler : IRequestHandler<UpdateAdCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IValidator<UpdateAdDto> _validator;

    public UpdateAdCommandHandler(
        DataContext context,
        IMapper mapper,
        ICurrentUserAccessor currentUserAccessor,
        IValidator<UpdateAdDto> validator)
    {
        _context = context;
        _mapper = mapper;
        _currentUserAccessor = currentUserAccessor;
        _validator = validator;
    }

    public async Task<Result<Unit>> Handle(UpdateAdCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request.UpdateAdDto);
        if (!validationResult.IsValid)
            return Result<Unit>.ValidationError(validationResult.Errors);

        var ad = await _context.Ads
            .Include(x => x.AppUser)
            .Include(x => x.Car)
            .FirstOrDefaultAsync(x => x.Id == request.Id);
        if (ad is null)
            return Result<Unit>.Empty();

        var currUsername = _currentUserAccessor.GetCurrentUsername();

        if (ad.AppUser.UserName != currUsername)
            return Result<Unit>.Failure("Users can update only their own ads.");

        _mapper.Map(request.UpdateAdDto, ad);

        var result = await _context.SaveChangesAsync() > 0;
        if (!result)
            return Result<Unit>.Failure("Failed to update ad.");

        return Result<Unit>.Success(Unit.Value);
    }
}