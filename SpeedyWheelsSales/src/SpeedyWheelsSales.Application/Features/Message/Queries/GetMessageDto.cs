namespace SpeedyWheelsSales.Application.Features.Message.Queries;

public class GetMessageDto
{
    public int Id { get; set; }
    public string SenderUsername { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
}