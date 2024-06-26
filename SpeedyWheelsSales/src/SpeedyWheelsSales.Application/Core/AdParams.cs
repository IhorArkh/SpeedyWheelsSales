﻿using Domain.Enums;

namespace SpeedyWheelsSales.Application.Core;

public class AdParams : PagingParams
{
    public string? QueryStringFromSavedSearch { get; set; }

    // Sorting params
    public string? SortColumn { get; set; }
    public string? SortOrder { get; set; }

    // Ad params
    public string? City { get; set; }

    // Car params
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MaxMileage { get; set; }
    public double? MinEngineSize { get; set; }
    public double? MaxEngineSize { get; set; }
    public double? MaxFuelConsumption { get; set; }
    public int? MinManufactureYear { get; set; }
    public int? MaxManufactureYear { get; set; }
    public EngineType? EngineType { get; set; }
    public Transmission? Transmission { get; set; }
    public TypeOfDrive? TypeOfDrive { get; set; }
    public Colors? Color { get; set; }

    public IDictionary<string, string> ToDictionary()
    {
        var dictionary = new Dictionary<string, string>();
        var properties = typeof(AdParams).GetProperties();

        foreach (var property in properties)
        {
            var value = property.GetValue(this);
            if (value != null)
            {
                if (property.Name == "QueryString")
                    continue;

                dictionary.Add(property.Name, value.ToString());
            }
        }

        return dictionary;
    }
}