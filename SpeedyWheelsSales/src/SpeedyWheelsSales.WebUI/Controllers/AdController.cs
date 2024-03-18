using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdDetails.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;

namespace SpeedyWheelsSales.WebUI.Controllers;

public class AdController : Controller
{
    private readonly HttpClient _httpClient;

    public AdController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("MyWebApi");
    }

    public async Task<IActionResult> ListAds()
    {
        var response = await _httpClient.GetAsync("Ad");

        string responseData = await response.Content.ReadAsStringAsync();

        var adList = JsonConvert.DeserializeObject<List<AdDto>>(responseData);

        return View(adList);
    }

    public async Task<IActionResult> GetAdDetails(int id)
    {
        var response = await _httpClient.GetAsync($"Ad/{id}");
        
        string responseData = await response.Content.ReadAsStringAsync();
        
        var adDetails = JsonConvert.DeserializeObject<AdDetailsDto>(responseData);
        
        return View(adDetails);
    }
}