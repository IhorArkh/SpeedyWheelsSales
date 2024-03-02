using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Queries.GetAdList;

public class GetAdListQueryHandler : IRequestHandler<GetAdListQuery, Result<List<Domain.Ad>>>
{
    private readonly DataContext _context;

    public GetAdListQueryHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<Result<List<Domain.Ad>>> Handle(GetAdListQuery request, CancellationToken cancellationToken)
    {
        var ads = await _context.Ads.ToListAsync();
        
        return Result<List<Domain.Ad>>.Success(ads);
    }
}