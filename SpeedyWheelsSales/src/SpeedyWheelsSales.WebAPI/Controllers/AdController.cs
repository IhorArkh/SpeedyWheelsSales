using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd;
using SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Commands.DeleteAd;
using SpeedyWheelsSales.Application.Features.Ad.Commands.MarkAdAsSold;
using SpeedyWheelsSales.Application.Features.Ad.Commands.ToggleFavouriteAd;
using SpeedyWheelsSales.Application.Features.Ad.Commands.UpdateAd;
using SpeedyWheelsSales.Application.Features.Ad.Commands.UpdateAd.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdDetails;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetFavouriteAds;

namespace SpeedyWheelsSales.WebAPI.Controllers;

public class AdController : BaseApiController
{
    /// <summary>
    /// Get sorted, filtered and paginated ads.
    /// </summary>
    /// <response code="200">Returns list of sorted, filtered and paginated ads.</response>
    [HttpGet]
    public async Task<IActionResult> GetAds([FromQuery] AdParams adParams)
    {
        return HandlePagedResult(await Mediator.Send(new GetAdListQuery { AdParams = adParams }));
    }

    /// <summary>
    /// Get ad by Id.
    /// </summary>
    /// <response code="200">Returns ad with corresponding Id.</response>
    /// <response code="404">If ad does not exist.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAdDetails(int id)
    {
        return HandleResult(await Mediator.Send(new GetAdDetailsQuery { Id = id }));
    }

    /// <summary>
    /// Create new ad.(authorized)
    /// </summary>
    /// <response code="200">If ad created successfully.</response>
    /// <response code="400">
    /// If validation errors occured.
    /// If couldn't get current user username.
    /// If user with current username was not found in Db.
    /// If failed to create ad.
    /// </response>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAd(CreateAdDto createAdDto)
    {
        return HandleResult(await Mediator.Send(new CreateAdCommand { CreateAdDto = createAdDto }));
    }

    /// <summary>
    /// Update existing ad.(authorized)
    /// </summary>
    /// <response code="200">If ad updated successfully.</response>
    /// <response code="400">
    /// If validation errors occured.
    /// If ad for updating was not found in Db.
    /// If user is not author of ad.
    /// If failed to update ad.
    /// </response>
    [Authorize]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateAd(int id, UpdateAdDto updateAdDto)
    {
        return HandleResult(await Mediator.Send(new UpdateAdCommand { UpdateAdDto = updateAdDto, Id = id }));
    }

    /// <summary>
    /// Delete existing ad.(authorized)
    /// </summary>
    /// <response code="200">If ad deleted successfully.</response>
    /// <response code="400">
    /// If ad for deleting was not found in Db.
    /// If user is not author of ad.
    /// If failed to update ad.
    /// </response>
    [Authorize]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteAd(int id)
    {
        return HandleResult(await Mediator.Send(new DeleteAdCommand { Id = id }));
    }

    /// <summary>
    /// Mark existing ad as sold.(authorized)
    /// </summary>
    /// <response code="200">If marked as sold successfully.</response>
    /// <response code="400">
    /// If ad for marking was not found in Db.
    /// If user is not author of ad.
    /// If failed to mark ad as sold.
    /// </response>
    [Authorize]
    [HttpPut("markAsSold/{id}")]
    public async Task<IActionResult> MarkAsSold(int id)
    {
        return HandleResult(await Mediator.Send(new MarkAdAsSoldCommand { Id = id }));
    }

    /// <summary>
    /// Add or remove ad from favourites.(authorized)
    /// </summary>
    /// <response code="200">If added/removed successfully.</response>
    /// <response code="400">
    /// If ad was not found in Db.
    /// If failed to get user username.
    /// If failed to add/remove ad.
    /// </response>
    [Authorize]
    [HttpPost("toggleFavourite/{id}")]
    public async Task<IActionResult> ToggleFavourite(int id)
    {
        return HandleResult(await Mediator.Send(new ToggleFavouriteAdCommand() { AdId = id }));
    }

    /// <summary>
    /// Get favourite ads for current user.
    /// </summary>
    /// <response code="200">Returns favourite ads of current user.</response>
    /// <response code="404">If couldn't get current user username.</response>
    [Authorize]
    [HttpGet("favourites")]
    public async Task<IActionResult> GetFavouriteAds([FromQuery] PagingParams pagingParams)
    {
        return HandlePagedResult(await Mediator.Send(new GetFavouriteAdsQuery { PagingParams = pagingParams }));
    }
}