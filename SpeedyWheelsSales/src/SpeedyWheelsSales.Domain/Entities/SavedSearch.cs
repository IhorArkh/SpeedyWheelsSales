using Domain.Entities;

namespace Domain;

public class SavedSearch
{
    public int Id { get; set; }
    public string Filters { get; set; }

    public string AppUserId { get; set; }
    
    public AppUser AppUser { get; set; }
}