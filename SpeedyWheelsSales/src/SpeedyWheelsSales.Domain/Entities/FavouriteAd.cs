using Domain.Entities;

namespace Domain;

public class FavouriteAd
{
    public int Id { get; set; }
    
    public int AdId { get; set; }
    public string AppUserId { get; set; }
    
    public Ad Ad { get; set; }
    public AppUser AppUser { get; set; }
}