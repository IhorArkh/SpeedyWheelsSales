using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpeedyWheelsSales.Application.Features.SavedSearch.Commands.DeleteSearch;
using SpeedyWheelsSales.Application.Features.SavedSearch.Commands.SaveSearch;

namespace SpeedyWheelsSales.WebAPI.Controllers;

public class SavedSearchController : BaseApiController
{
    /// <summary>
    /// Save search.(authorized)
    /// </summary>
    /// <response code="200">If saved successfully.</response>
    /// <response code="400">
    /// If can't find user in db.
    /// If no params provided ot can't get query string.
    /// </response>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SaveSearch([FromQuery] SaveSearchParams saveSearchParams)
    {
        return HandleResult(await Mediator.Send(new SaveSearchCommand { SaveSearchParams = saveSearchParams }));
    }

    /// <summary>
    /// Delete search.(authorized)
    /// </summary>
    /// <response code="200">If deleted successfully.</response>
    /// <response code="400">
    /// If can't find user in db.
    /// If can't find search by provided id in db.
    /// </response>
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSearch(int id)
    {
        return HandleResult(await Mediator.Send(new DeleteSearchCommand { Id = id }));
    }
}