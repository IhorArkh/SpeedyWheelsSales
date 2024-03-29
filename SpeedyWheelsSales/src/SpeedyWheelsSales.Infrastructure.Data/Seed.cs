﻿using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace SpeedyWheelsSales.Infrastructure.Data;

public class Seed
{
    public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
    {
        if (!context.AppUsers.Any())
        {
            var user1 = new AppUser
            {
                UserName = "user1",
                Email = "mail@test.com",
                PhoneNumber = "380500813839",
                Name = "John Doe",
                Location = "City1",
                Bio = "Lorem Ipsum",
                PhotoUrl =
                    "https://www.shutterstock.com/shutterstock/photos/1669189411/display_1500/stock-photo-gardening-home-girl-replanting-green-pasture-in-home-garden-indoor-garden-room-with-plants-banner-1669189411.jpg"
            };
            await userManager.CreateAsync(user1, "Password123");

            var user2 = new AppUser
            {
                UserName = "user2",
                Email = "mail2@test.com",
                PhoneNumber = "380500813838",
                Name = "Jane Smith",
                Location = "City2",
                Bio = "Dolor Sit Amet",
                PhotoUrl =
                    "https://www.shutterstock.com/shutterstock/photos/577183882/display_1500/stock-photo-software-developer-programming-code-abstract-computer-script-code-programming-code-screen-of-577183882.jpg"
            };
            await userManager.CreateAsync(user2, "Password123");

            var ads = new List<Ad>
            {
                new Ad
                {
                    Description = "Car for sale 1",
                    City = "City1",
                    IsSold = false,
                    Car = new Car
                    {
                        Model = "Model1",
                        Price = 15000,
                        Mileage = 50000,
                        EngineSize = 2,
                        FuelConsumption = 8.5,
                        ManufactureDate = new DateTime(2019, 1, 1),
                        EngineType = EngineType.Diesel,
                        Transmission = Transmission.Automatic,
                        TypeOfDrive = Domain.Enums.TypeOfDrive.Fwd,
                        Color = Colors.Blue,
                        Vin = "vin1",
                        Plates = "AX7777AX"
                    },
                    AppUser = user1,
                    Photos = new List<Photo>
                    {
                        new Photo
                        {
                            Url =
                                "https://www.shutterstock.com/shutterstock/photos/577183882/display_1500/stock-photo-software-developer-programming-code-abstract-computer-script-code-programming-code-screen-of-577183882.jpg",
                            IsMain = true
                        },
                        new Photo
                        {
                            Url =
                                "https://www.shutterstock.com/shutterstock/photos/1669189411/display_1500/stock-photo-gardening-home-girl-replanting-green-pasture-in-home-garden-indoor-garden-room-with-plants-banner-1669189411.jpg",
                            IsMain = false
                        },
                    }
                },
                new Ad
                {
                    Description = "Car for sale 2",
                    City = "City2",
                    IsSold = true,
                    SoldAt = DateTime.UtcNow.AddDays(-10),
                    Car = new Car
                    {
                        Model = "Model2",
                        Price = 20000,
                        Mileage = 30000,
                        EngineSize = 2.5,
                        FuelConsumption = 9.0,
                        ManufactureDate = new DateTime(2020, 3, 1),
                        EngineType = EngineType.Diesel,
                        Transmission = Transmission.Manual,
                        TypeOfDrive = Domain.Enums.TypeOfDrive.Rwd,
                        Color = Colors.Red,
                        Vin = "vin2",
                        Plates = "AX1777AX"
                    },
                    AppUser = user2,
                    Photos = new List<Photo>
                    {
                        new Photo
                        {
                            Url =
                                "https://www.shutterstock.com/shutterstock/photos/577183882/display_1500/stock-photo-software-developer-programming-code-abstract-computer-script-code-programming-code-screen-of-577183882.jpg",
                            IsMain = true
                        },
                        new Photo
                        {
                            Url =
                                "https://www.shutterstock.com/shutterstock/photos/1669189411/display_1500/stock-photo-gardening-home-girl-replanting-green-pasture-in-home-garden-indoor-garden-room-with-plants-banner-1669189411.jpg",
                            IsMain = false
                        },
                    }
                },
                new Ad
                {
                    Description = "Car for sale 3",
                    City = "City3",
                    IsSold = false,
                    Car = new Car
                    {
                        Model = "Model3",
                        Price = 18000,
                        Mileage = 40000,
                        EngineSize = 2.2,
                        FuelConsumption = 8.0,
                        ManufactureDate = new DateTime(2018, 5, 1),
                        EngineType = EngineType.Electric,
                        Transmission = Transmission.Automatic,
                        TypeOfDrive = Domain.Enums.TypeOfDrive.Rwd,
                        Color = Colors.Silver,
                        Vin = "vin3",
                        Plates = "AI7777AX"
                    },
                    AppUser = user1,
                    Photos = new List<Photo>
                    {
                        new Photo
                        {
                            Url =
                                "https://www.shutterstock.com/shutterstock/photos/577183882/display_1500/stock-photo-software-developer-programming-code-abstract-computer-script-code-programming-code-screen-of-577183882.jpg",
                            IsMain = true
                        },
                        new Photo
                        {
                            Url =
                                "https://www.shutterstock.com/shutterstock/photos/1669189411/display_1500/stock-photo-gardening-home-girl-replanting-green-pasture-in-home-garden-indoor-garden-room-with-plants-banner-1669189411.jpg",
                            IsMain = false
                        },
                    }
                },
                new Ad
                {
                    Description = "Car for sale 4",
                    City = "City4",
                    IsSold = false,
                    Car = new Car
                    {
                        Model = "Model4",
                        Price = 22000,
                        Mileage = 35000,
                        EngineSize = 2.8,
                        FuelConsumption = 10.0,
                        ManufactureDate = new DateTime(2017, 8, 1),
                        EngineType = EngineType.Electric,
                        Transmission = Transmission.Automatic,
                        TypeOfDrive = Domain.Enums.TypeOfDrive.Fwd,
                        Color = Colors.Black,
                        Vin = "vin4",
                        Plates = "AX7757AX"
                    },
                    AppUser = user2,
                    Photos = new List<Photo>
                    {
                        new Photo
                        {
                            Url =
                                "https://www.shutterstock.com/shutterstock/photos/577183882/display_1500/stock-photo-software-developer-programming-code-abstract-computer-script-code-programming-code-screen-of-577183882.jpg",
                            IsMain = true
                        },
                        new Photo
                        {
                            Url =
                                "https://www.shutterstock.com/shutterstock/photos/1669189411/display_1500/stock-photo-gardening-home-girl-replanting-green-pasture-in-home-garden-indoor-garden-room-with-plants-banner-1669189411.jpg",
                            IsMain = false
                        },
                    }
                },
                new Ad
                {
                    Description = "Car for sale 5",
                    City = "City5",
                    IsSold = false,
                    Car = new Car
                    {
                        Model = "Model5",
                        Price = 25000,
                        Mileage = 30000,
                        EngineSize = 3.0,
                        FuelConsumption = 9.5,
                        ManufactureDate = new DateTime(2019, 10, 1),
                        EngineType = EngineType.Gas,
                        Transmission = Transmission.Automatic,
                        TypeOfDrive = Domain.Enums.TypeOfDrive.Awd,
                        Color = Colors.White,
                        Vin = "vin6",
                        Plates = "AX7777YX"
                    },
                    AppUser = user1,
                    Photos = new List<Photo>
                    {
                        new Photo
                        {
                            Url =
                                "https://www.shutterstock.com/shutterstock/photos/577183882/display_1500/stock-photo-software-developer-programming-code-abstract-computer-script-code-programming-code-screen-of-577183882.jpg",
                            IsMain = true
                        },
                        new Photo
                        {
                            Url =
                                "https://www.shutterstock.com/shutterstock/photos/1669189411/display_1500/stock-photo-gardening-home-girl-replanting-green-pasture-in-home-garden-indoor-garden-room-with-plants-banner-1669189411.jpg",
                            IsMain = false
                        },
                    }
                }
            };

            await context.AddRangeAsync(ads);
            await context.SaveChangesAsync();
        }
    }
}