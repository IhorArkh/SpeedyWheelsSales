using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppUser : IdentityUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Location { get; set; }
    public string? Bio { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.UtcNow;
    
    public ICollection<Ad> Ads { get; set; } = new List<Ad>();
    public ICollection<SavedSearch> FavouriteSearches { get; set; } = new List<SavedSearch>();

    public ICollection<FavouriteAd> FavouriteAds { get; set; } = new List<FavouriteAd>();
    // Fields like PhoneNumber, IsConfirmed, Email will be provided by asp.net identity 
}