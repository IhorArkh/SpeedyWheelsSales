using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SpeedyWheelsSales.WebUI.Interfaces;
using SpeedyWheelsSales.WebUI.Models;

namespace SpeedyWheelsSales.WebUI.Services;

public class ErrorHandlingService : IErrorHandlingService
{
    public async Task<IActionResult> HandleErrorResponseAsync(HttpResponseMessage response)
    {
        var errorMessage = response.ReasonPhrase ?? "";

        if (response.Content != null)
            errorMessage = await response.Content.ReadAsStringAsync();

        var errorViewModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id,
            StatusCode = (int)response.StatusCode,
            ErrorMessage = errorMessage
        };

        return new ViewResult
        {
            ViewName = "Error",
            ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = errorViewModel
            }
        };
    }
}