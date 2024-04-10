using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpeedyWheelsSales.Application.Features.Profile.Commands.ChangeProfilePhoto;
using SpeedyWheelsSales.Application.Features.Profile.Commands.UpdateUserProfile;
using SpeedyWheelsSales.Application.Features.Profile.Commands.UpdateUserProfile.DTOs;
using SpeedyWheelsSales.Application.Features.Profile.Queries.GetUserProfile;
using SpeedyWheelsSales.Application.Features.Profile.Queries.GetUserProfile.DTOs;

namespace SpeedyWheelsSales.WebAPI.Controllers;

public class ProfileController : BaseApiController
{
    /// <summary>
    /// Get user profile provided username. Or current user profile if username wasn't provided. 
    /// </summary>
    /// <response code="200">Returns user profile.</response>
    /// <response code="404">
    /// If can't define both current user username or other user username.
    /// If user with corresponding username wasn't found in db.
    /// </response>
    [HttpGet]
    public async Task<ActionResult<List<UserProfileDto>>> GetUserProfile([FromQuery] string? username)
    {
        return HandleResult(await Mediator.Send(new GetUserProfileQuery { Username = username }));
    }

    /// <summary>
    /// Update user profile.(authorized)
    /// </summary>
    /// <response code="200">If user profile updated successfully.</response>
    /// <response code="404">
    /// If can't define current user username.
    /// If user with corresponding username wasn't found in db.
    /// </response>
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateUserProfile(UpdateUserProfileDto updateUserProfileDto)
    {
        return HandleResult(await Mediator.Send(new UpdateUserProfileCommand
            { UpdateUserProfileDto = updateUserProfileDto }));
    }

    /// <summary>
    /// Change profile photo.(authorized)
    /// </summary>
    /// <response code="200">If photo changed successfully.</response>
    /// <response code="400">
    /// If occured error deleting previous photo.
    /// If occured error adding new photo.
    /// </response>
    /// <response code="404">
    /// If can't define current user username.
    /// If user with corresponding username wasn't found in db.
    /// </response>
    [Authorize]
    [HttpPut("changePhoto")]
    public async Task<IActionResult> ChangeProfilePhoto(IFormFile photo)
    {
        return HandleResult(await Mediator.Send(new ChangeProfilePhotoCommand { Photo = photo }));
    }
}