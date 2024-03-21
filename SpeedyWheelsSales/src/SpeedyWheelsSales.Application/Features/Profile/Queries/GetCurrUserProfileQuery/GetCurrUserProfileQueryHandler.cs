using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Profile.Queries.GetCurrUserProfileQuery.DTOs;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Profile.Queries.GetCurrUserProfileQuery;

public class GetCurrUserProfileQueryHandler : IRequestHandler<GetCurrUserProfileQuery, Result<CurrUserProfileDto>>
{
    private readonly DataContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IMapper _mapper;

    public GetCurrUserProfileQueryHandler(DataContext context, ICurrentUserAccessor currentUserAccessor, IMapper mapper)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
        _mapper = mapper;
    }

    public async Task<Result<CurrUserProfileDto>> Handle(GetCurrUserProfileQuery request,
        CancellationToken cancellationToken)
    {
        var currUserUsername = _currentUserAccessor.GetCurrentUsername();
        if (currUserUsername is null)
            return Result<CurrUserProfileDto>.Empty();

        var appUser = await _context.AppUsers
            .Include(x => x.Ads)
            .ThenInclude(x => x.Car)
            .Include(x => x.Ads)
            .ThenInclude(x => x.Photo)
            .FirstOrDefaultAsync(x => x.UserName == currUserUsername);

        if (appUser is null)
            return Result<CurrUserProfileDto>.Empty();

        var currUserProfileDto = _mapper.Map<CurrUserProfileDto>(appUser);

        return Result<CurrUserProfileDto>.Success(currUserProfileDto);
    }
}