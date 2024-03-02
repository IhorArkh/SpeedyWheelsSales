using Domain;
using MediatR;
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
        return await Mediator.Send(new GetAdListQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Ad>> GetAdDetails(int id)
    {
        return await Mediator.Send(new GetAdDetailsQuery { Id = id });
    }

    [HttpPost]
    public async Task<IActionResult> CreateAd(Ad ad)
    {
        await Mediator.Send(new CreateAdCommand { Ad = ad });
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAd(int id, Ad ad)
    {
        ad.Id = id;

        await Mediator.Send(new UpdateAdCommand { Ad = ad });
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAd(int id)
    {
        await Mediator.Send(new DeleteAdCommand { Id = id });
        return Ok();
    }
}