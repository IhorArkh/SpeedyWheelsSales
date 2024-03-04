﻿using Domain.Enums;

namespace SpeedyWheelsSales.Application.Ad.Commands.CreateAd.DTOs;

public class CreateAdCarDto
{
    public string Model { get; set; }
    public decimal Price { get; set; }
    public int Mileage { get; set; }
    public decimal EngineSize { get; set; }
    public double FuelConsumption { get; set; }
    public DateTime ManufactureDate { get; set; }
    public EngineType EngineType { get; set; }
    public Transmission Transmission { get; set; }
    public TypeOfDrive TypeOfDrive { get; set; }
    public Colors Color { get; set; }
}