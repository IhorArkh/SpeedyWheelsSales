namespace SpeedyWheelsSales.Application.Features.Profile.Commands.UpdateUserProfile.DTOs;

public class UpdateUserProfileDto
{
    public string Name { get; set; }
    public string Location { get; set; }
    public string? Bio { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}