namespace SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdDetails.DTOs;

public class AdDetailsDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    public bool IsSold { get; set; }
    public bool IsAuthor { get; set; }
    public DateTime? SoldAt { get; set; }
    public AdDetailsCarDto CarDto { get; set; }
    public ICollection<AdDetailsPhotoDto> PhotoDtos { get; set; } = new List<AdDetailsPhotoDto>();
    public AdDetailsAppUserDto AppUserDto { get; set; }
}