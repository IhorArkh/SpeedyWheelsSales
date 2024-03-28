using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpeedyWheelsSales.Application.Features.SavedSearch.SaveSearch;

namespace SpeedyWheelsSales.WebAPI.Controllers;

public class SavedSearchController : BaseApiController
{
    /// <summary>
    /// Save search.(authorized)
    /// </summary>
    /// <response code="200">If saved successfully.</response>
    /// <response code="400">
    /// If can't define current user username.
    /// If no params provided ot can't get query string.
    /// </response>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SaveSearch([FromQuery] SaveSearchParams saveSearchParams)
    {
        return HandleResult(await Mediator.Send(new SaveSearchCommand { SaveSearchParams = saveSearchParams }));
    }
}