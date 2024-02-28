using Domain;
using Domain.Enums;
using DriveType = Domain.Enums.DriveType;

namespace SpeedyWheelsSales.Infrastructure.Data;

public class Seed
{
    public static async Task SeedData(DataContext context)
    {
        if (context.AppUsers.Any())
            return;

        var users = new List<AppUser>
        {
            new AppUser
            {
                Name = "John",
                Surname = "Doe",
                Location = "City A",
                Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                PhotoUrl = "https://example.com/photo1.jpg",
                RegisterDate = DateTime.UtcNow,
                Ads = new List<Ad>
                {
                    new Ad
                    {
                        Description = "Car for sale",
                        City = "City A",
                        IsSold = false,
                        CreatedAt = DateTime.UtcNow,
                        Car = new Car
                        {
                            Model = "Toyota",
                            Price = 25000,
                            Mileage = 50000,
                            EngineSize = 2.0m,
                            FuelConsumption = 8.5,
                            ManufactureDate = DateTime.Parse("2020-01-01"),
                            EngineType = EngineType.Gas,
                            Transmission = Transmission.Automatic,
                            DriveType = DriveType.Fwd,
                            Color = Colors.Red
                        },
                        Photo = new List<Photo>
                        {
                            new Photo { Url = "https://example.com/photo1.jpg", IsMain = 1 },
                            new Photo { Url = "https://example.com/photo2.jpg", IsMain = 0 },
                        }
                    }
                },
                FavouriteSearches = new List<SavedSearch>
                {
                    new SavedSearch { Filters = "Filter1" },
                    new SavedSearch { Filters = "Filter2" }
                }
            },
            new AppUser
            {
                Name = "Jane",
                Surname = "Doe",
                Location = "City B",
                Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                PhotoUrl = "https://example.com/photo2.jpg",
                RegisterDate = DateTime.UtcNow,
                Ads = new List<Ad>
                {
                    new Ad
                    {
                        Description = "Another car for sale",
                        City = "City B",
                        IsSold = false,
                        CreatedAt = DateTime.UtcNow,
                        Car = new Car
                        {
                            Model = "Honda",
                            Price = 20000,
                            Mileage = 40000,
                            EngineSize = 1.8m,
                            FuelConsumption = 7.5,
                            ManufactureDate = DateTime.Parse("2019-01-01"),
                            EngineType = EngineType.Diesel,
                            Transmission = Transmission.Manual,
                            DriveType = DriveType.Rwd,
                            Color = Colors.Blue
                        },
                        Photo = new List<Photo>
                        {
                            new Photo { Url = "https://example.com/photo3.jpg", IsMain = 1 },
                            new Photo { Url = "https://example.com/photo4.jpg", IsMain = 0 },
                        }
                    }
                },
                FavouriteSearches = new List<SavedSearch>
                {
                    new SavedSearch { Filters = "Filter3" }
                }
            },
            new AppUser
            {
                Name = "Alex",
                Surname = "Smith",
                Location = "City C",
                Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                PhotoUrl = "https://example.com/photo3.jpg",
                RegisterDate = DateTime.UtcNow,
                Ads = new List<Ad>
                {
                    new Ad
                    {
                        Description = "Used car for sale",
                        City = "City C",
                        IsSold = false,
                        CreatedAt = DateTime.UtcNow,
                        Car = new Car
                        {
                            Model = "Ford",
                            Price = 18000,
                            Mileage = 60000,
                            EngineSize = 1.5m,
                            FuelConsumption = 9.0,
                            ManufactureDate = DateTime.Parse("2018-01-01"),
                            EngineType = EngineType.Electric,
                            Transmission = Transmission.Automatic,
                            DriveType = DriveType.Fwd,
                            Color = Colors.Black
                        },
                        Photo = new List<Photo>
                        {
                            new Photo { Url = "https://example.com/photo5.jpg", IsMain = 1 },
                            new Photo { Url = "https://example.com/photo6.jpg", IsMain = 0 },
                        }
                    },
                    new Ad
                    {
                        Description = "SUV for sale",
                        City = "City C",
                        IsSold = false,
                        CreatedAt = DateTime.UtcNow,
                        Car = new Car
                        {
                            Model = "Jeep",
                            Price = 30000,
                            Mileage = 40000,
                            EngineSize = 2.5m,
                            FuelConsumption = 10.0,
                            ManufactureDate = DateTime.Parse("2017-01-01"),
                            EngineType = EngineType.Diesel,
                            Transmission = Transmission.Automatic,
                            DriveType = DriveType.Awd,
                            Color = Colors.Green
                        },
                        Photo = new List<Photo>
                        {
                            new Photo { Url = "https://example.com/photo7.jpg", IsMain = 1 },
                            new Photo { Url = "https://example.com/photo8.jpg", IsMain = 0 },
                        }
                    }
                }
            }
        };
        await context.AppUsers.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }
}