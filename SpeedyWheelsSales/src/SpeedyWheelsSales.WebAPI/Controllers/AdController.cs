using Domain.Entities;
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
    public async Task<IActionResult> CreateAd(CreateAdDto createAdDto)
    {
        return HandleResult(await Mediator.Send(new CreateAdCommand { CreateAdDto = createAdDto }));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAd(int id, UpdateAdDto updateAdDto)
    {
        return HandleResult(await Mediator.Send(new UpdateAdCommand { UpdateAdDto = updateAdDto, Id = id}));
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAd(int id)
    {
        return HandleResult(await Mediator.Send(new DeleteAdCommand { Id = id }));
    }
}