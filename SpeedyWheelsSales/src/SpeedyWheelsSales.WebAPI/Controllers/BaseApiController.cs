using MediatR;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Retry;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.WebAPI.Extensions;

namespace SpeedyWheelsSales.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    protected AsyncRetryPolicy<ActionResult> RetryPolicy { get; init; }

    public BaseApiController()
    {
        RetryPolicy = Policy
            .HandleResult<ActionResult>(result => !(result is OkObjectResult))
            .RetryAsync(3);
    }

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.ValidationErrors.Count != default)
        {
            return BadRequest(new
            {
                errors = result.ValidationErrors.Select(x => new { x.AttemptedValue, x.ErrorMessage })
            });
        }

        if (result.IsEmpty)
            return NotFound();

        if (result.IsSuccess && result.Value != null)
            return Ok(result.Value);

        if (result.IsSuccess && result.Value == null)
            return NotFound();

        return BadRequest(result.Error);
    }

    protected ActionResult HandlePagedResult<T>(Result<PagedList<T>> result)
    {
        if (result.ValidationErrors.Count != default)
        {
            return BadRequest(new
            {
                errors = result.ValidationErrors.Select(x => new { x.AttemptedValue, x.ErrorMessage })
            });
        }

        if (result.IsEmpty)
            return NotFound();

        if (result.IsSuccess && result.Value != null)
        {
            Response.AddPaginationHeader(result.Value.CurrentPage, result.Value.PageSize,
                result.Value.TotalCount, result.Value.TotalPages);
            return Ok(result.Value);
        }

        if (result.IsSuccess && result.Value == null)
            return NotFound();

        return BadRequest(result.Error);
    }
}