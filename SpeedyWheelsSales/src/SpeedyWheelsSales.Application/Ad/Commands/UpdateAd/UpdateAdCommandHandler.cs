﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Interfaces;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Commands.UpdateAd;

public class UpdateAdCommandHandler : IRequestHandler<UpdateAdCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IUserAccessor _userAccessor;

    public UpdateAdCommandHandler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
    {
        _context = context;
        _mapper = mapper;
        _userAccessor = userAccessor;
    }

    public async Task<Result<Unit>> Handle(UpdateAdCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var ad = await _context.Ads
                .Include(x => x.AppUser)
                .Include(x => x.Car)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (ad is null)
                return null;

            if (ad.AppUser.UserName != _userAccessor.GetUsername())
                return Result<Unit>.Failure("Users can update only their own ads.");

            _mapper.Map(request.UpdateAdDto, ad);
            
            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
                return Result<Unit>.Failure("Failed to update ad.");

            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return Result<Unit>.Failure("An error occurred while updating the ad.");
        }
    }
}