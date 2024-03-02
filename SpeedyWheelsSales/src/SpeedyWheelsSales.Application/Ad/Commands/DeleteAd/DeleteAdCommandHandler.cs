using MediatR;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Commands.DeleteAd;

public class DeleteAdCommandHandler : IRequestHandler<DeleteAdCommand, Result<Unit>>
{
    private readonly DataContext _context;

    public DeleteAdCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteAdCommand request, CancellationToken cancellationToken)
    {
        var ad = await _context.Ads.FindAsync(request.Id);

        if (ad is null)
            return null;

        _context.Remove(ad);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
            return Result<Unit>.Failure("Failed to delete ad.");

        return Result<Unit>.Success(Unit.Value);
    }
}