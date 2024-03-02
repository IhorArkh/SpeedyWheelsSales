using MediatR;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Commands.DeleteAd;

public class DeleteAdCommandHandler : IRequestHandler<DeleteAdCommand>
{
    private readonly DataContext _context;

    public DeleteAdCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteAdCommand request, CancellationToken cancellationToken)
    {
        var ad = await _context.Ads.FindAsync(request.Id);

        _context.Remove(ad);
        await _context.SaveChangesAsync();
    }
}