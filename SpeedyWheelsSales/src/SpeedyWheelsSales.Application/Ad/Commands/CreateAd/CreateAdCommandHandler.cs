using MediatR;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Commands.CreateAd;

public class CreateAdCommandHandler : IRequestHandler<CreateAdCommand, Result<Unit>>
{
    private readonly DataContext _context;

    public CreateAdCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(CreateAdCommand request, CancellationToken cancellationToken)
    {
        _context.Add(request.Ad);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
            return Result<Unit>.Failure("Failed to create ad.");

        return Result<Unit>.Success(Unit.Value);
    }
}