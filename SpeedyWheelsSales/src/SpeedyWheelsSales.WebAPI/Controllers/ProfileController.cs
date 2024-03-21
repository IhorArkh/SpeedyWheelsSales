using Microsoft.AspNetCore.Mvc;
using SpeedyWheelsSales.Application.Features.Profile.Queries.GetCurrUserProfileQuery;
using SpeedyWheelsSales.Application.Features.Profile.Queries.GetCurrUserProfileQuery.DTOs;

namespace SpeedyWheelsSales.WebAPI.Controllers;

public class ProfileController : BaseApiController
{
    /// <summary>
    /// Get current user profile.
    /// </summary>
    /// <response code="200">Returns current user profile.</response>
    /// <response code="400">
    /// If can't get current user username.
    /// If user with corresponding username wasn't found in db.
    /// </response>
    [HttpGet]
    public async Task<ActionResult<List<CurrUserProfileDto>>> GetCurrUserProfile()
    {
        return HandleResult(await Mediator.Send(new GetCurrUserProfileQuery()));
    }
}