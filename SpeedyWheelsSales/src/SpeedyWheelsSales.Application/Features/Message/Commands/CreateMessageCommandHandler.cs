using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Message.Queries;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Message.Commands;

public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, Result<CreateMessageDto>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CreateMessageCommandHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<CreateMessageDto>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var sender = await _context.AppUsers
            .FirstOrDefaultAsync(x => x.UserName == request.CurrUserUsername);
        if (sender is null)
            return Result<CreateMessageDto>.Empty();

        var recipient = await _context.AppUsers
            .FirstOrDefaultAsync(x => x.UserName == request.RecipientUsername);
        if (recipient is null)
            return Result<CreateMessageDto>.Empty();

        var message = new Domain.Entities.Message
        {
            SenderId = sender.Id,
            Sender = sender,
            RecipientId = recipient.Id,
            Recipient = recipient,
            Content = request.Content,
            SentAt = DateTime.UtcNow
        };

        _context.Messages.Add(message);

        var result = await _context.SaveChangesAsync() > 0;
        if (!result)
            return Result<CreateMessageDto>.Failure("Failed to save message");

        var messageDto = _mapper.Map<CreateMessageDto>(message);

        return Result<CreateMessageDto>.Success(messageDto);
    }
}