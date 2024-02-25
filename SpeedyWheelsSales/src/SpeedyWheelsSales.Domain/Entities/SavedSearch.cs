namespace Domain;

public class SavedSearch
{
    public int Id { get; set; }
    public string Filters { get; set; }

    public int AppUserId { get; set; }
    
    public AppUser AppUser { get; set; }
}