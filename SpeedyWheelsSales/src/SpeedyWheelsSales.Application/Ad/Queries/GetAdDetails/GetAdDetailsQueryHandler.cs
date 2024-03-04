﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Ad.Queries.GetAdDetails.DTOs;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Queries.GetAdDetails;

public class GetAdDetailsQueryHandler : IRequestHandler<GetAdDetailsQuery, Result<AdDetailsDto>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetAdDetailsQueryHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Result<AdDetailsDto>> Handle(GetAdDetailsQuery request, CancellationToken cancellationToken)
    {
        // var ad = await _context.Ads
        //     .Where(x => x.Id == request.Id)
        //     .ProjectTo<AdDetailsDto>(_mapper.ConfigurationProvider)
        //     .FirstOrDefaultAsync();

        var ad = await _context.Ads
            .Include(x => x.AppUser)
            .Include(x => x.Photo)
            .Include(x => x.Car)
            .FirstOrDefaultAsync(x => x.Id == request.Id);
        
        // Which approach is better?

        var adDetailsDto = _mapper.Map<AdDetailsDto>(ad);
        
        return Result<AdDetailsDto>.Success(adDetailsDto);
    }
}