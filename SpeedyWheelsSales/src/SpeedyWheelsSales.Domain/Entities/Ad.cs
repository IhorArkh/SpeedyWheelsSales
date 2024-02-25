namespace Domain;

public class Ad
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsSold { get; set; }
    public DateTime SoldAt { get; set; }
    
    public int AppUserId { get; set; }

    public Car Car { get; set; }
    public ICollection<Photo> Photo { get; set; } = new List<Photo>();
    public AppUser AppUser { get; set; }
}