﻿using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd;
using SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Commands.DeleteAd;
using SpeedyWheelsSales.Application.Features.Ad.Commands.UpdateAd;
using SpeedyWheelsSales.Application.Features.Ad.Commands.UpdateAd.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdDetails;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList;

namespace SpeedyWheelsSales.WebAPI.Controllers;

public class AdController : BaseApiController
{
    /// <summary>
    /// Get all ads.
    /// </summary>
    /// <response code="200">Returns list of all ads.</response>
    [HttpGet]
    public async Task<ActionResult<List<Ad>>> GetAds()
    {
        return HandleResult(await Mediator.Send(new GetAdListQuery()));
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
    /// If user user is not author of ad.
    /// If failed to update ad.
    /// </response>
    [Authorize]
    [HttpPut("{id}")]
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
    /// If user user is not author of ad.
    /// If failed to update ad.
    /// </response>
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAd(int id)
    {
        return HandleResult(await Mediator.Send(new DeleteAdCommand { Id = id }));
    }
}