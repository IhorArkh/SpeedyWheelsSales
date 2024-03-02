using AutoMapper;
using MediatR;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Commands.UpdateAd;

public class UpdateAdCommandHandler : IRequestHandler<UpdateAdCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UpdateAdCommandHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<Unit>> Handle(UpdateAdCommand request, CancellationToken cancellationToken)
    {
        var ad = await _context.Ads.FindAsync(request.Ad.Id);

        if (ad is null)
            return null;

        _mapper.Map(request.Ad, ad);

        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
            return Result<Unit>.Failure("Failed to update ad.");

        return Result<Unit>.Success(Unit.Value);
    }
}