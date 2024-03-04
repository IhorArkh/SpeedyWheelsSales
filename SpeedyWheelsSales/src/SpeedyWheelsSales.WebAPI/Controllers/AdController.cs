using System.Reflection.Metadata;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SpeedyWheelsSales.Application.Ad.Commands.CreateAd;
using SpeedyWheelsSales.Application.Ad.Commands.DeleteAd;
using SpeedyWheelsSales.Application.Ad.Commands.UpdateAd;
using SpeedyWheelsSales.Application.Ad.Queries.GetAdDetails;
using SpeedyWheelsSales.Application.Ad.Queries.GetAdList;

namespace SpeedyWheelsSales.WebAPI.Controllers;

public class AdController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<Ad>>> GetAds()
    {
        return HandleResult(await Mediator.Send(new GetAdListQuery()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAdDetails(int id)
    {
        return HandleResult(await Mediator.Send(new GetAdDetailsQuery { Id = id }));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAd(Ad ad)
    {
        return HandleResult(await Mediator.Send(new CreateAdCommand { Ad = ad }));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAd(int id, Ad ad)
    {
        ad.Id = id;

        return HandleResult(await Mediator.Send(new UpdateAdCommand { Ad = ad }));
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAd(int id)
    {
        return HandleResult(await Mediator.Send(new DeleteAdCommand { Id = id }));
    }
}