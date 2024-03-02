﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SpeedyWheelsSales.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
}