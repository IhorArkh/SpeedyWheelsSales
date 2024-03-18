using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Commands.UpdateAd;

public class UpdateAdCommandHandler : IRequestHandler<UpdateAdCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public UpdateAdCommandHandler(DataContext context, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _mapper = mapper;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<Unit>> Handle(UpdateAdCommand request, CancellationToken cancellationToken)
    {
        var ad = await _context.Ads
            .Include(x => x.AppUser)
            .Include(x => x.Car)
            .FirstOrDefaultAsync(x => x.Id == request.Id);
        if (ad is null)
            return Result<Unit>.Empty();

        if (ad.AppUser.UserName != _currentUserAccessor.GetCurrentUsername())
            return Result<Unit>.Failure("Users can update only their own ads.");

        _mapper.Map(request.UpdateAdDto, ad);

        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
            return Result<Unit>.Failure("Failed to update ad.");

        return Result<Unit>.Success(Unit.Value);
    }
}