using MediatR;
using Microsoft.AspNetCore.SignalR;
using SpeedyWheelsSales.Application.Features.Message.Commands;
using SpeedyWheelsSales.Application.Features.Message.Queries;

namespace SpeedyWheelsSales.WebAPI.SignalR;

public class ChatHub : Hub
{
    private readonly IMediator _mediator;

    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task SendMessage(CreateMessageCommand command)
    {
        var message = await _mediator.Send(command);

        var groupName = GetGroupName(command.CurrUserUsername, command.RecipientUsername);

        await Clients.Group(groupName).SendAsync("ReceiveMessage", message.Value);
    }

    public async Task GetChatMessages(GetMessagesQuery query)
    {
        var messages = await _mediator.Send(query);

        await Clients.Caller.SendAsync("ReceiveChatMessages", messages.Value);
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();

        var currUserUsername = httpContext.Request.Query["currUserUsername"].ToString();
        var recipientUsername = httpContext.Request.Query["recipientUsername"].ToString();

        var groupName = GetGroupName(currUserUsername, recipientUsername);

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        await GetChatMessages(new GetMessagesQuery
        {
            CurrUserUsername = currUserUsername,
            RecipientUsername = recipientUsername
        });
    }

    private string GetGroupName(string currUserUsername, string recipientUsername)
    {
        int comparisonResult = string.Compare(currUserUsername, recipientUsername);

        if (comparisonResult < 0)
            return $"{currUserUsername}_{recipientUsername}";

        return $"{recipientUsername}_{currUserUsername}";
    }
}