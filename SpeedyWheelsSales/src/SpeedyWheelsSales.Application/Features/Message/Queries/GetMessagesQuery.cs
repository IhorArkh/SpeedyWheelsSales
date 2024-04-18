using MediatR;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Features.Message.Queries;

public class GetMessagesQuery : IRequest<Result<List<GetMessageDto>>>
{
    public string CurrUserUsername { get; set; }
    public string RecipientUsername { get; set; }
}