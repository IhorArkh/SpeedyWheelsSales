using MediatR;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Message.Queries;

namespace SpeedyWheelsSales.Application.Features.Message.Commands;

public class CreateMessageCommand : IRequest<Result<CreateMessageDto>>
{
    public string RecipientUsername { get; set; }
    public string Content { get; set; }
    public string CurrUserUsername { get; set; }
}