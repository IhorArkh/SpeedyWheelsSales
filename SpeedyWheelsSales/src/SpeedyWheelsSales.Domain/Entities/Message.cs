﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Domain.Entities;

public class Message
{
    public int Id { get; set; }
    public string SenderId { get; set; }
    public AppUser Sender { get; set; }
    public string RecipientId { get; set; }
    public AppUser Recipient { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
}