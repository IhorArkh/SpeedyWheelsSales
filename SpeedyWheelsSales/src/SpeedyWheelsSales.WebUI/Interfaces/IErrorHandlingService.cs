using Microsoft.AspNetCore.Mvc;

namespace SpeedyWheelsSales.WebUI.Interfaces;

public interface IErrorHandlingService
{
    Task<IActionResult> HandleErrorResponseAsync(HttpResponseMessage response);
}