namespace Domain;

public class FavouriteAd
{
    public int Id { get; set; }

    public int AppUserId { get; set; }

    public AppUser AppUser { get; set; }
}