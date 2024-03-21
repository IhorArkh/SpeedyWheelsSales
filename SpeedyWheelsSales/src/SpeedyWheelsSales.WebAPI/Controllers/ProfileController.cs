using Microsoft.AspNetCore.Mvc;
using SpeedyWheelsSales.Application.Features.Profile.Queries.GetCurrUserProfileQuery;
using SpeedyWheelsSales.Application.Features.Profile.Queries.GetCurrUserProfileQuery.DTOs;

namespace SpeedyWheelsSales.WebAPI.Controllers;

public class ProfileController : BaseApiController
{
    /// <summary>
    /// Get user profile provided username. Or current user profile if username wasn't provided. 
    /// </summary>
    /// <response code="200">Returns user profile.</response>
    /// <response code="400">
    /// If can't define both current user username or other user username.
    /// If user with corresponding username wasn't found in db.
    /// </response>
    [HttpGet]
    public async Task<ActionResult<List<UserProfileDto>>> GetUserProfile([FromQuery]string? username)
    {
        return HandleResult(await Mediator.Send(new GetUserProfileQuery {Username = username}));
    }
}