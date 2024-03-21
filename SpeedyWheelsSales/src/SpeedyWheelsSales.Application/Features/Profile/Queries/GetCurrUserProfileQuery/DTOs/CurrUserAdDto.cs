using System.Collections;

namespace SpeedyWheelsSales.Application.Features.Profile.Queries.GetCurrUserProfileQuery.DTOs;

public class CurrUserAdDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsSold { get; set; }
    public DateTime? SoldAt { get; set; }

    public CurrUserCarDto CarDto { get; set; }
    public ICollection<CurrUserPhotoDto> PhotoDtos { get; set; }
}