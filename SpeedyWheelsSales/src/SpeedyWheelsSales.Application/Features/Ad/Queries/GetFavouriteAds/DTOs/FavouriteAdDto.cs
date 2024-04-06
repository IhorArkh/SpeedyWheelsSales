namespace SpeedyWheelsSales.Application.Features.Ad.Queries.GetFavouriteAds.DTOs;

public class FavouriteAdDto
{
    public string City { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    public bool IsSold { get; set; }
    public DateTime? SoldAt { get; set; }
    public int AdId { get; set; }
    public FavouriteAdCarDto CarDto { get; set; }
    public ICollection<FavouriteAdPhotoDto> PhotoDtos { get; set; } = new List<FavouriteAdPhotoDto>();
}