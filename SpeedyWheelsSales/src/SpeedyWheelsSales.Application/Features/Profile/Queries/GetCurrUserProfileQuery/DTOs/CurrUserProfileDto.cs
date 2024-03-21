using System.Collections;

namespace SpeedyWheelsSales.Application.Features.Profile.Queries.GetCurrUserProfileQuery.DTOs;

public class CurrUserProfileDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string? Bio { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.UtcNow;

    public ICollection<CurrUserAdDto> AdDtos { get; set; } = new List<CurrUserAdDto>();
}