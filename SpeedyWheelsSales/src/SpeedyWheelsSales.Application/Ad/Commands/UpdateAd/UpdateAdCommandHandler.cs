using AutoMapper;
using MediatR;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Commands.UpdateAd;

public class UpdateAdCommandHandler : IRequestHandler<UpdateAdCommand>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UpdateAdCommandHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task Handle(UpdateAdCommand request, CancellationToken cancellationToken)
    {
        var ad = await _context.Ads.FindAsync(request.Ad.Id);

        _mapper.Map(request.Ad, ad);
        
        await _context.SaveChangesAsync();
    }
}