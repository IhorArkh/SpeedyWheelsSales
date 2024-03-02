using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Queries.GetAdList;

public class GetAdListQueryHandler : IRequestHandler<GetAdListQuery, List<Domain.Ad>>
{
    private readonly DataContext _context;

    public GetAdListQueryHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<List<Domain.Ad>> Handle(GetAdListQuery request, CancellationToken cancellationToken)
    {
        return await _context.Ads.ToListAsync();
    }
}