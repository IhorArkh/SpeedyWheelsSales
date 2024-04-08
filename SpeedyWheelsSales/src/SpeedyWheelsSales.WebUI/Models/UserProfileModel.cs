using SpeedyWheelsSales.Application.Features.Profile.Queries.GetUserProfile.DTOs;

namespace SpeedyWheelsSales.WebUI.Models;

public class UserProfileModel
{
    public UserProfileDto UserProfileDto { get; set; }
    public bool IsOwner { get; set; }
}