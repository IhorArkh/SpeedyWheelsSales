using Domain;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using SpeedyWheelsSales.Infrastructure.Data;

public class Seed
{
    public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
    {
        if (!context.AppUsers.Any())
        {
            var user1 = new AppUser
            {
                UserName = "user1",
                Name = "John Doe",
                Location = "City1",
                Bio = "Lorem Ipsum",
                PhotoUrl = "https://example.com/photo1.jpg"
            };
            await userManager.CreateAsync(user1, "Password123");

            var user2 = new AppUser
            {
                UserName = "user2",
                Name = "Jane Smith",
                Location = "City2",
                Bio = "Dolor Sit Amet",
                PhotoUrl = "https://example.com/photo2.jpg"
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
                        EngineSize = 2.0m,
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
                    Photo = new List<Photo>
                    {
                        new Photo { Url = "https://example.com/photo1_1.jpg", IsMain = 1 },
                        new Photo { Url = "https://example.com/photo1_2.jpg", IsMain = 0 },
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
                        EngineSize = 2.5m,
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
                    Photo = new List<Photo>
                    {
                        new Photo { Url = "https://example.com/photo2_1.jpg", IsMain = 1 },
                        new Photo { Url = "https://example.com/photo2_2.jpg", IsMain = 0 },
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
                        EngineSize = 2.2m,
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
                    Photo = new List<Photo>
                    {
                        new Photo { Url = "https://example.com/photo3_1.jpg", IsMain = 1 },
                        new Photo { Url = "https://example.com/photo3_2.jpg", IsMain = 0 },
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
                        EngineSize = 2.8m,
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
                    Photo = new List<Photo>
                    {
                        new Photo { Url = "https://example.com/photo4_1.jpg", IsMain = 1 },
                        new Photo { Url = "https://example.com/photo4_2.jpg", IsMain = 0 },
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
                        EngineSize = 3.0m,
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
                    Photo = new List<Photo>
                    {
                        new Photo { Url = "https://example.com/photo5_1.jpg", IsMain = 1 },
                        new Photo { Url = "https://example.com/photo5_2.jpg", IsMain = 0 },
                    }
                }
            };

            await context.AddRangeAsync(ads);
            await context.SaveChangesAsync();
        }
    }
}