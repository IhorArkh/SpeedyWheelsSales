using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Interfaces;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Message.Queries;

public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, Result<List<GetMessageDto>>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetMessagesQueryHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<List<GetMessageDto>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserName == request.CurrUserUsername);
        if (user is null)
            return Result<List<GetMessageDto>>.Empty();

        var recipient = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserName == request.RecipientUsername);
        if (recipient is null)
            return Result<List<GetMessageDto>>.Empty();

        var messages = await _context.Messages
            .Where(x => (x.SenderId == user.Id && x.RecipientId == recipient.Id) ||
                        (x.SenderId == recipient.Id && x.RecipientId == user.Id))
            .ToListAsync();

        var messagesDtos = _mapper.Map<List<GetMessageDto>>(messages);

        return Result<List<GetMessageDto>>.Success(messagesDtos);
    }
}