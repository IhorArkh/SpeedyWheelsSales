using System.Diagnostics;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdDetails.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetFavouriteAds.DTOs;
using SpeedyWheelsSales.WebUI.Models;

namespace SpeedyWheelsSales.WebUI.Controllers;

public class AdController : Controller
{
    private readonly HttpClient _httpClient;

    public AdController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("MyWebApi");
    }

    public async Task<IActionResult> ListAds(AdParams adParams)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        if (token != null)
            _httpClient.SetBearerToken(token);

        var queryString = QueryHelpers.AddQueryString("", adParams.ToDictionary());

        var response = await _httpClient.GetAsync($"Ad{queryString}");

        string responseData = await response.Content.ReadAsStringAsync();

        var adList = JsonConvert.DeserializeObject<List<AdListDto>>(responseData);

        var viewModel = new AdListViewModel
        {
            Ads = adList,
            AdParams = adParams
        };

        var paginationData = JObject.Parse(response.Headers.GetValues("pagination").First());
        viewModel.CurrentPage = paginationData.Value<int>("currentPage");
        viewModel.ItemsPerPage = paginationData.Value<int>("itemsPerPage");
        viewModel.TotalItems = paginationData.Value<int>("totalItems");
        viewModel.TotalPages = paginationData.Value<int>("totalPages");

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetAdDetails(int id)
    {
        var response = await _httpClient.GetAsync($"Ad/{id}");

        string responseData = await response.Content.ReadAsStringAsync();

        var adDetails = JsonConvert.DeserializeObject<AdDetailsDto>(responseData);

        return View(adDetails);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetFavourites()
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        _httpClient.SetBearerToken(token);

        var response = await _httpClient.GetAsync($"Ad/favourites");

        if (!response.IsSuccessStatusCode)
        {
            var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View("Error", errorViewModel);
        }

        string responseData = await response.Content.ReadAsStringAsync();

        var favouriteAds = JsonConvert.DeserializeObject<List<FavouriteAdDto>>(responseData);

        return View(favouriteAds);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ToggleFavourite([FromForm] int adId)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        _httpClient.SetBearerToken(token);

        var response = await _httpClient.PostAsync($"Ad/toggleFavourite/{adId}", null);

        if (!response.IsSuccessStatusCode)
        {
            var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View("Error", errorViewModel);
        }

        return RedirectToAction("GetFavourites");
    }
}