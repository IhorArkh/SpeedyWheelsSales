﻿namespace SpeedyWheelsSales.WebAPI.DTOs;

public class RegisterDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
}