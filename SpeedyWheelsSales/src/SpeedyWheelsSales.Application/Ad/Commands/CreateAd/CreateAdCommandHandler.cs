using MediatR;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Commands.CreateAd;

public class CreateAdCommandHandler : IRequestHandler<CreateAdCommand>
{
    private readonly DataContext _context;

    public CreateAdCommandHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task Handle(CreateAdCommand request, CancellationToken cancellationToken)
    {
        _context.Add(request.Ad);
        await _context.SaveChangesAsync();
    }
}