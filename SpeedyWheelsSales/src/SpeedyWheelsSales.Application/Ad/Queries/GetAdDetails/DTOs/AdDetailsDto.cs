namespace SpeedyWheelsSales.Application.Ad.Queries.GetAdDetails.DTOs;

public class AdDetailsDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    public bool IsSold { get; set; }
    public DateTime? SoldAt { get; set; }
    public CarDetailsDto CarDetailsDto { get; set; }
    public ICollection<PhotoDetailsDto> PhotoDetailsDtos { get; set; } = new List<PhotoDetailsDto>();
    public AppUserDetailsDto AppUserDetailsDto { get; set; }
}