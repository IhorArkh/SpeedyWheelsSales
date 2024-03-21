using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Profile.Queries.GetCurrUserProfileQuery.DTOs;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Profile.Queries.GetCurrUserProfileQuery;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, Result<UserProfileDto>>
{
    private readonly DataContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IMapper _mapper;

    public GetUserProfileQueryHandler(DataContext context, ICurrentUserAccessor currentUserAccessor, IMapper mapper)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
        _mapper = mapper;
    }

    public async Task<Result<UserProfileDto>> Handle(GetUserProfileQuery request,
        CancellationToken cancellationToken)
    {
        var username = request.Username ?? _currentUserAccessor.GetCurrentUsername();

        if (username is null)
            return Result<UserProfileDto>.Empty();

        var appUser = await _context.AppUsers
            .Include(x => x.Ads)
            .ThenInclude(x => x.Car)
            .Include(x => x.Ads)
            .ThenInclude(x => x.Photos)
            .FirstOrDefaultAsync(x => x.UserName == username);

        if (appUser is null)
            return Result<UserProfileDto>.Empty();

        var currUserProfileDto = _mapper.Map<UserProfileDto>(appUser);

        return Result<UserProfileDto>.Success(currUserProfileDto);
    }
}