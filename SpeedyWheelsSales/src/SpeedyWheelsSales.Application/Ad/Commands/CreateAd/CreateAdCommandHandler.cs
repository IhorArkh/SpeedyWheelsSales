using AutoMapper;
using MediatR;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Commands.CreateAd;

public class CreateAdCommandHandler : IRequestHandler<CreateAdCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CreateAdCommandHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<Unit>> Handle(CreateAdCommand request, CancellationToken cancellationToken)
    {
        var ad = _mapper.Map<Domain.Ad>(request.CreateAdDto);
        
        _context.Add(ad);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
            return Result<Unit>.Failure("Failed to create ad.");

        return Result<Unit>.Success(Unit.Value);
    }
}