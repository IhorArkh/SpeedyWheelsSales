using MediatR;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Queries.GetAdDetails;

public class GetAdDetailsQueryHandler : IRequestHandler<GetAdDetailsQuery, Domain.Ad>
{
    private readonly DataContext _context;

    public GetAdDetailsQueryHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<Domain.Ad> Handle(GetAdDetailsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Ads.FindAsync(request.Id);
    }
}