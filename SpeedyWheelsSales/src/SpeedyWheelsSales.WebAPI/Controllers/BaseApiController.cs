using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.ValidationErrors != null)
        {
            var errorMessages = new List<ValidationError>();

            foreach (var error in result.ValidationErrors)
            {
                errorMessages.Add(new ValidationError()
                {
                    AttemptedValue = error.AttemptedValue.ToString(),
                    ErrorMessage = error.ErrorMessage,
                    PropertyName = error.PropertyName.Split('.').LastOrDefault()
                });
            }

            return BadRequest(errorMessages);
        }

        if (result.IsEmpty)
            return NotFound();

        if (result.IsSuccess && result.Value != null)
            return Ok(result.Value);

        if (result.IsSuccess && result.Value == null)
            return NotFound();

        return BadRequest(result.Error);
    }
}