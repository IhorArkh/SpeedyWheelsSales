using MediatR;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Queries.GetAdDetails;

public class GetAdDetailsQueryHandler : IRequestHandler<GetAdDetailsQuery, Result<Domain.Ad>>
{
    private readonly DataContext _context;

    public GetAdDetailsQueryHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<Result<Domain.Ad>> Handle(GetAdDetailsQuery request, CancellationToken cancellationToken)
    {
        var ad = await _context.Ads.FindAsync(request.Id);
        
        return Result<Domain.Ad>.Success(ad);
    }
}