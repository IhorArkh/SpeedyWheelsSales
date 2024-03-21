namespace Domain.Entities;

public class Ad
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsSold { get; set; }
    public DateTime? SoldAt { get; set; }
    public string AppUserId { get; set; }

    public ICollection<FavouriteAd> FavouriteAds { get; set; }
    public Car Car { get; set; }
    public ICollection<Photo> Photo { get; set; } = new List<Photo>(); // TODO Need to change it to "Photos"
    public AppUser AppUser { get; set; }
}