using System.Diagnostics;
using AutoMapper;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.SavedSearch.Commands.SaveSearch;
using SpeedyWheelsSales.Application.Features.SavedSearch.Queries.GetSavedSearches;
using SpeedyWheelsSales.WebUI.Interfaces;

namespace SpeedyWheelsSales.WebUI.Controllers;

public class SavedSearchController : Controller
{
    private readonly IMapper _mapper;
    private readonly IErrorHandlingService _errorHandlingService;
    private readonly HttpClient _httpClient;

    public SavedSearchController(
        IHttpClientFactory httpClientFactory,
        IMapper mapper,
        IErrorHandlingService errorHandlingService)
    {
        _mapper = mapper;
        _errorHandlingService = errorHandlingService;
        _httpClient = httpClientFactory.CreateClient("MyWebApi");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SaveSearch(AdParams adParams)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        _httpClient.SetBearerToken(token);

        var saveSearchParams = _mapper.Map<SaveSearchParams>(adParams);

        var queryString = QueryHelpers.AddQueryString("", saveSearchParams.ToDictionary());

        var response = await _httpClient.PostAsync($"SavedSearch{queryString}", null);
        if (!response.IsSuccessStatusCode)
            return await _errorHandlingService.HandleErrorResponseAsync(response);

        return RedirectToAction("ListAds", "Ad");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetSavedSearches()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        _httpClient.SetBearerToken(token);

        var response = await _httpClient.GetAsync("SavedSearch");
        if (!response.IsSuccessStatusCode)
            return await _errorHandlingService.HandleErrorResponseAsync(response);

        string responseData = await response.Content.ReadAsStringAsync();

        var savedSearchesList = JsonConvert.DeserializeObject<List<SavedSearchDto>>(responseData);

        return View(savedSearchesList);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> DeleteSearch([FromForm] int searchId)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        _httpClient.SetBearerToken(token);

        var response = await _httpClient.DeleteAsync($"SavedSearch/{searchId}");

        if (!response.IsSuccessStatusCode)
            return await _errorHandlingService.HandleErrorResponseAsync(response);

        return RedirectToAction("GetSavedSearches");
    }
}