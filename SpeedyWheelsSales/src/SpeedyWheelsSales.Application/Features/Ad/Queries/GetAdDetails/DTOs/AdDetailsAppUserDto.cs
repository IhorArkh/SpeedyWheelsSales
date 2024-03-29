﻿namespace SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdDetails.DTOs;

public class AdDetailsAppUserDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string? Bio { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.UtcNow;
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
}