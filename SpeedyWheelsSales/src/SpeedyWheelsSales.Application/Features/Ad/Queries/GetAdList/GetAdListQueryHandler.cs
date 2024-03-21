using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList;

public class GetAdListQueryHandler : IRequestHandler<GetAdListQuery, Result<List<AdListDto>>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetAdListQueryHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Result<List<AdListDto>>> Handle(GetAdListQuery request, CancellationToken cancellationToken)
    {
        var ads = await _context.Ads
            .Include(x => x.Car)
            .Include(x => x.Photos)
            .ToListAsync();

        var adDtos = _mapper.Map<List<AdListDto>>(ads);
        
        return Result<List<AdListDto>>.Success(adDtos);
    }
}