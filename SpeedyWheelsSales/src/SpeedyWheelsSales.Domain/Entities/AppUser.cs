namespace Domain;

public class AppUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public ICollection<Ad> Ads { get; set; } = new List<Ad>();
    public Photo Photo { get; set; }
    // Fields like PhoneNumber, IsConfirmed, Email will be provided by asp.net identity 
}