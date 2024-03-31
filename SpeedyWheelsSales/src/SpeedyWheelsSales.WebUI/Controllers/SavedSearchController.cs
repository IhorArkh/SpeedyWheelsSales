using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.SavedSearch.Commands.SaveSearch;

namespace SpeedyWheelsSales.WebUI.Controllers;

public class SavedSearchController : Controller
{
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;

    public SavedSearchController(IHttpClientFactory httpClientFactory, IMapper mapper)
    {
        _mapper = mapper;
        _httpClient = httpClientFactory.CreateClient("MyWebApi");
    }

    public async Task<IActionResult> SaveSearch(AdParams adParams)
    {
        var saveSearchParams = _mapper.Map<SaveSearchParams>(adParams);

        var queryString = QueryHelpers.AddQueryString("", saveSearchParams.ToDictionary());

        var response = await _httpClient.PostAsync($"SavedSearch{queryString}", null);

        if (response.IsSuccessStatusCode)
        {
            return Ok(new { success = true, message = "Search saved successfully!" });
        }
        else
        {
            return BadRequest(new { success = false, message = "Failed to save search." });
        }
    }
}