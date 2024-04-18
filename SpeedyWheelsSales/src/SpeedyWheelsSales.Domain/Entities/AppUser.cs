using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppUser : IdentityUser
{
    public string Name { get; set; }
    public string Location { get; set; }
    public string? Bio { get; set; }
    public string? PhotoPublicId { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime RegisterDate { get; set; }

    public ICollection<Ad> Ads { get; set; } = new List<Ad>();
    public ICollection<SavedSearch> SavedSearches { get; set; } = new List<SavedSearch>();
    public ICollection<FavouriteAd> FavouriteAds { get; set; } = new List<FavouriteAd>();
    public ICollection<Message> SentMessages { get; set; } = new List<Message>();
    public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
}