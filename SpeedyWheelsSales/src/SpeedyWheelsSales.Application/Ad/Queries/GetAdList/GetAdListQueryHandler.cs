using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Ad.Queries.GetAdList.DTOs;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Queries.GetAdList;

public class GetAdListQueryHandler : IRequestHandler<GetAdListQuery, Result<List<AdDto>>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetAdListQueryHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Result<List<AdDto>>> Handle(GetAdListQuery request, CancellationToken cancellationToken)
    {
        var ads = await _context.Ads
            .Include(x => x.Car)
            .Include(x => x.Photo)
            .ToListAsync();

        var adDtos = _mapper.Map<List<AdDto>>(ads);
        
        return Result<List<AdDto>>.Success(adDtos);
    }
}