using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Commands.AddAdPhoto;
using SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd;
using SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Commands.DeleteAd;
using SpeedyWheelsSales.Application.Features.Ad.Commands.DeleteAdPhoto;
using SpeedyWheelsSales.Application.Features.Ad.Commands.MarkAdAsSold;
using SpeedyWheelsSales.Application.Features.Ad.Commands.SetMainAdPhoto;
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
    /// If occured error adding photo.
    /// If failed to save changes to db.
    /// </response>
    /// <response code="404">
    /// If couldn't get current user username.
    /// If user with current username was not found in Db.
    /// </response>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAd(CreateAdDto createAdDto)
    {
        return HandleResult(await Mediator.Send(new CreateAdCommand { CreateAdDto = createAdDto }));
    }

    //TODO Update it after adding update ad photos functionality.
    /// <summary>
    /// Update existing ad.(authorized)
    /// </summary>
    /// <response code="200">If ad updated successfully.</response>
    /// <response code="400">
    /// If validation errors occured.
    /// If user is not author of ad.
    /// If failed to save changes to db.
    /// </response>
    /// <response code="404">If ad for updating was not found in Db.</response>
    [Authorize]
    [HttpPut("update/{id}")] // TODO think about changing this and delete routes to just "{id}"
    public async Task<IActionResult> UpdateAd(int id, UpdateAdDto updateAdDto)
    {
        return HandleResult(await Mediator.Send(new UpdateAdCommand { UpdateAdDto = updateAdDto, Id = id }));
    }

    /// <summary>
    /// Delete existing ad.(authorized)
    /// </summary>
    /// <response code="200">If ad deleted successfully.</response>
    /// <response code="400">
    /// If user is not author of ad.
    /// If failed to save changes to db.
    /// </response>
    /// <response code="404">If ad for deleting was not found in Db.</response>
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
    /// If user is not author of ad.
    /// If failed to save changes to db.
    /// </response>
    /// <response code="404">If ad for marking was not found in Db.</response>
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
    /// If failed to save changes to db.
    /// </response>
    /// <response code="404">
    /// If ad was not found in Db.
    /// If failed to get user username.
    /// </response>
    [Authorize]
    [HttpPost("toggleFavourite/{id}")]
    public async Task<IActionResult> ToggleFavourite(int id)
    {
        return HandleResult(await Mediator.Send(new ToggleFavouriteAdCommand() { AdId = id }));
    }

    /// <summary>
    /// Get favourite ads for current user. (authorized)
    /// </summary>
    /// <response code="200">Returns favourite ads of current user.</response>
    /// <response code="400">If failed to save changes to db.</response>
    /// <response code="404">If couldn't get current user username.</response>
    [Authorize]
    [HttpGet("favourites")]
    public async Task<IActionResult> GetFavouriteAds([FromQuery] PagingParams pagingParams)
    {
        return HandlePagedResult(await Mediator.Send(new GetFavouriteAdsQuery { PagingParams = pagingParams }));
    }

    /// <summary>
    /// Delete ad photo. (authorized)
    /// </summary>
    /// <response code="200">When deleted successfully.</response>
    /// <response code="400">
    /// When failed to save changes to db.
    /// When current user username != ad author username.
    /// When tried to delete main photo.
    /// When occured error deleting photo.
    /// </response>
    /// <response code="404">
    /// If couldn't get current user username.
    /// When photo not found in db.
    /// </response>
    [Authorize]
    [HttpDelete("photo/{photoId}")]
    public async Task<IActionResult> DeleteAdPhoto(string photoId)
    {
        return HandleResult(await Mediator.Send(new DeleteAdPhotoCommand { PhotoId = photoId }));
    }

    /// <summary>
    /// Set main ad photo. (authorized)
    /// </summary>
    /// <response code="200">When set as main successfully.</response>
    /// <response code="400">
    /// When failed to save changes to db.
    /// When current user username != ad author username.
    /// </response>
    /// <response code="404">
    /// If couldn't get current user username.
    /// When photo not found in db.
    /// </response>
    [Authorize]
    [HttpPut("photo/{photoId}")]
    public async Task<IActionResult> SetMainAdPhoto(string photoId)
    {
        return HandleResult(await Mediator.Send(new SetMainAdPhotoCommand { PhotoId = photoId }));
    }

    /// <summary>
    /// Add ad photo. (authorized)
    /// </summary>
    /// <response code="200">When added successfully.</response>
    /// <response code="400">
    /// When failed to save changes to db.
    /// When current user username != ad author username.
    /// When occured error adding photo.
    /// </response>
    /// <response code="404">
    /// If couldn't get current user username.
    /// When ad not found in db.
    /// </response>
    [Authorize]
    [HttpPost("photo/{adId}")]
    public async Task<IActionResult> AddAdPhoto(int adId, IFormFile photo)
    {
        return HandleResult(await Mediator.Send(new AddAdPhotoCommand { AdId = adId, Photo = photo }));
    }
}