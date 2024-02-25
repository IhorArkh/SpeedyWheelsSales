using System.Security.AccessControl;

namespace Domain;

public class AppUser
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Location { get; set; }
    public string? Bio { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

    public FavouriteAd FavouriteAd { get; set; }
    public ICollection<Ad> Ads { get; set; } = new List<Ad>();
    public ICollection<SavedSearch> FavouriteSearches { get; set; } = new List<SavedSearch>();
    // Fields like PhoneNumber, IsConfirmed, Email will be provided by asp.net identity 
}