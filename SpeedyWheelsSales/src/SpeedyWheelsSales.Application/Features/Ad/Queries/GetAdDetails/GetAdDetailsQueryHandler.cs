using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdDetails.DTOs;
using SpeedyWheelsSales.Application.Interfaces;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdDetails;

public class GetAdDetailsQueryHandler : IRequestHandler<GetAdDetailsQuery, Result<AdDetailsDto>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public GetAdDetailsQueryHandler(DataContext context, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _mapper = mapper;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<AdDetailsDto>> Handle(GetAdDetailsQuery request, CancellationToken cancellationToken)
    {
        var ad = await _context.Ads
            .Include(x => x.AppUser)
            .Include(x => x.Photos)
            .Include(x => x.Car)
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (ad is null)
            return Result<AdDetailsDto>.Empty();

        var adDetailsDto = _mapper.Map<AdDetailsDto>(ad);

        var currUsername = _currentUserAccessor.GetCurrentUsername();
        if (currUsername == ad.AppUser.UserName)
            adDetailsDto.IsAuthor = true;

        return Result<AdDetailsDto>.Success(adDetailsDto);
    }
}