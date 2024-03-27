namespace SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;

public class AdListDto
{
    public int Id { get; set; }
    public string City { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    public bool IsSold { get; set; }
    public DateTime? SoldAt { get; set; }
    public AdListCarDto CarDto { get; set; }
    public ICollection<AdListPhotoDto> PhotoDtos { get; set; } = new List<AdListPhotoDto>();
}