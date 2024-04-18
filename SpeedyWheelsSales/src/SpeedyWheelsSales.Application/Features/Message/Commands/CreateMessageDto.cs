﻿namespace SpeedyWheelsSales.Application.Features.Message.Queries;

public class CreateMessageDto
{
    public int Id { get; set; }
    public string SenderId { get; set; }
    public string RecipientId { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
}