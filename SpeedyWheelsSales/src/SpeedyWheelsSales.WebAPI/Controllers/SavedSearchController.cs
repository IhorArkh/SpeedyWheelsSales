using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpeedyWheelsSales.Application.Features.SavedSearch.Commands.DeleteSearch;
using SpeedyWheelsSales.Application.Features.SavedSearch.Commands.SaveSearch;
using SpeedyWheelsSales.Application.Features.SavedSearch.Queries.GetSavedSearches;

namespace SpeedyWheelsSales.WebAPI.Controllers;

public class SavedSearchController : BaseApiController
{
    /// <summary>
    /// Get saved searches.(authorized)
    /// </summary>
    /// <response code="200">Returns list of saved searches.</response>
    /// <response code="404">
    /// If can't find user in db.
    /// </response>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetSavedSearches()
    {
        return HandleResult(await Mediator.Send(new GetSavedSearchesQuery()));
    }

    /// <summary>
    /// Save search.(authorized)
    /// </summary>
    /// <response code="200">If saved successfully.</response>
    /// <response code="400">
    /// If no params provided ot can't get query string.
    /// If the same search is already exists
    /// </response>
    /// <response code="404">
    /// If can't find user in db.
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
    /// <response code="404">
    /// If can't find user in db.
    /// If can't find search by provided id in db.
    /// </response>
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSavedSearch(int id)
    {
        return HandleResult(await Mediator.Send(new DeleteSearchCommand { Id = id }));
    }
}