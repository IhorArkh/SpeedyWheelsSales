﻿using Domain.Enums;

namespace SpeedyWheelsSales.Application.Features.Profile.Queries.GetUserProfile.DTOs;

public class UserCarDto
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public decimal Price { get; set; }
    public int Mileage { get; set; }
    public decimal EngineSize { get; set; }
    public string Vin { get; set; }
    public string Plates { get; set; }
    public DateTime ManufactureDate { get; set; }
    public EngineType EngineType { get; set; }
    public Transmission Transmission { get; set; }
    public TypeOfDrive TypeOfDrive { get; set; }
}