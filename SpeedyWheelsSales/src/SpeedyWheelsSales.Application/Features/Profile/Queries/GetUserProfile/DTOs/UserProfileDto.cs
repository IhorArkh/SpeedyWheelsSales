namespace SpeedyWheelsSales.Application.Features.Profile.Queries.GetUserProfile.DTOs;

public class UserProfileDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string? Bio { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

    public ICollection<UserAdDto> AdDtos { get; set; } = new List<UserAdDto>();
}