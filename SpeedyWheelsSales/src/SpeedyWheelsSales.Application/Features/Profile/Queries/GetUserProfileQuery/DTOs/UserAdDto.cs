using System.Collections;

namespace SpeedyWheelsSales.Application.Features.Profile.Queries.GetCurrUserProfileQuery.DTOs;

public class UserAdDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsSold { get; set; }
    public DateTime? SoldAt { get; set; }

    public UserCarDto CarDto { get; set; }
    public ICollection<UserPhotoDto> PhotoDtos { get; set; }
}