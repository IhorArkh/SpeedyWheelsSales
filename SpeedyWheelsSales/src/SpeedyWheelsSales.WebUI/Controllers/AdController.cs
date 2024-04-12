using System.Diagnostics;
using System.Text;
using AutoMapper;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Commands.UpdateAd.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdDetails.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetFavouriteAds.DTOs;
using SpeedyWheelsSales.WebUI.Models;

namespace SpeedyWheelsSales.WebUI.Controllers;

public class AdController : Controller
{
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;

    public AdController(IHttpClientFactory httpClientFactory, IMapper mapper)
    {
        _mapper = mapper;
        _httpClient = httpClientFactory.CreateClient("MyWebApi");
    }

    public async Task<IActionResult> ListAds(AdParams adParams, string? queryStrFromSavedSearch)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        if (token != null)
            _httpClient.SetBearerToken(token);

        if (queryStrFromSavedSearch != null)
            adParams.QueryStringFromSavedSearch = "&" + queryStrFromSavedSearch.Substring(1);

        var queryStringFromAdParams = QueryHelpers.AddQueryString("", adParams.ToDictionary());

        var response =
            await _httpClient.GetAsync($"Ad{queryStringFromAdParams}{adParams.QueryStringFromSavedSearch}");
        // When user try get saved search, queryStrFromSavedSearch will be passed to this method, than it will be used
        // instead of parameters from adParams which will be null in such case. Done for correct paging.

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
        var token = await HttpContext.GetTokenAsync("access_token");

        if (token != null)
            _httpClient.SetBearerToken(token);

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

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUpdateAdView(int adId)
    {
        var response = await _httpClient.GetAsync($"Ad/{adId}");

        if (!response.IsSuccessStatusCode)
        {
            var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View("Error", errorViewModel);
        }

        string responseData = await response.Content.ReadAsStringAsync();

        var adDetails = JsonConvert.DeserializeObject<AdDetailsDto>(responseData);

        var updateAdDto = _mapper.Map<UpdateAdDto>(adDetails);

        var updateAdVm = new UpdateAdViewModel()
        {
            AdId = adId,
            UpdateAdDto = updateAdDto,
            PhotoDtos = adDetails.PhotoDtos
        };

        return View(updateAdVm);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdateAd(UpdateAdViewModel updateAdVm)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        if (token != null)
            _httpClient.SetBearerToken(token);

        var jsonContent = JsonConvert.SerializeObject(updateAdVm.UpdateAdDto);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"Ad/update/{updateAdVm.AdId}", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View("Error", errorViewModel);
        }

        return RedirectToAction("GetProfile", "Profile");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> GetDeleteAdView(int adId)
    {
        var response = await _httpClient.GetAsync($"Ad/{adId}");

        if (!response.IsSuccessStatusCode)
        {
            var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View("Error", errorViewModel);
        }

        string responseData = await response.Content.ReadAsStringAsync();

        var adDetails = JsonConvert.DeserializeObject<AdDetailsDto>(responseData);

        return View(adDetails);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> DeleteAd(int adId)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        if (token != null)
            _httpClient.SetBearerToken(token);

        var response = await _httpClient.DeleteAsync($"Ad/delete/{adId}");

        if (!response.IsSuccessStatusCode)
        {
            var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View("Error", errorViewModel);
        }

        return RedirectToAction("GetProfile", "Profile");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetMarkAdAsSoldView(int adId)
    {
        return View(adId);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> MarkAdAsSold(int adId)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        if (token != null)
            _httpClient.SetBearerToken(token);

        var response = await _httpClient.PutAsync($"Ad/markAsSold/{adId}", null);

        if (!response.IsSuccessStatusCode)
        {
            var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View("Error", errorViewModel);
        }

        return RedirectToAction("GetProfile", "Profile");
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetCreateAdView()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAd(CreateAdDto createAdDto)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        if (token != null)
            _httpClient.SetBearerToken(token);

        var content = ConvertToMultipartFormDataContent(createAdDto);

        var response = await _httpClient.PostAsync("Ad", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View("Error", errorViewModel);
        }

        return RedirectToAction("GetProfile", "Profile");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> DeleteAdPhoto(string photoId)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        if (token != null)
            _httpClient.SetBearerToken(token);

        var response = await _httpClient.DeleteAsync($"Ad/photo/{photoId}");

        if (!response.IsSuccessStatusCode)
        {
            var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View("Error", errorViewModel);
        }

        return RedirectToAction("GetProfile", "Profile");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SetMainAdPhoto(string photoId)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        if (token != null)
            _httpClient.SetBearerToken(token);

        var response = await _httpClient.PutAsync($"Ad/photo/{photoId}", null);

        if (!response.IsSuccessStatusCode)
        {
            var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View("Error", errorViewModel);
        }

        return RedirectToAction("GetProfile", "Profile");
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetAddAdPhotoView(int adId)
    {
        return View(adId);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddAdPhoto(int adId, IFormFile photo)
    {
        if (photo is null)
            return BadRequest();

        var token = await HttpContext.GetTokenAsync("access_token");

        if (token != null)
            _httpClient.SetBearerToken(token);

        var content = new MultipartFormDataContent();
        var photoContent = new StreamContent(photo.OpenReadStream());
        content.Add(photoContent, "Photo", photo.FileName);

        var response = await _httpClient.PostAsync($"Ad/photo/{adId}", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View("Error", errorViewModel);
        }

        return RedirectToAction("GetProfile", "Profile");
    }

    private MultipartFormDataContent ConvertToMultipartFormDataContent(CreateAdDto createAdDto)
    {
        var content = new MultipartFormDataContent();

        if (createAdDto.Description != null)
            content.Add(new StringContent(createAdDto.Description), "Description");
        if (createAdDto.City != null)
            content.Add(new StringContent(createAdDto.City), "City");

        if (createAdDto.CreateAdCarDto != null)
        {
            var carDto = createAdDto.CreateAdCarDto;
            var carDtoProperties = typeof(CreateAdCarDto).GetProperties();

            foreach (var property in carDtoProperties)
            {
                var value = property.GetValue(carDto);
                if (value != null)
                {
                    content.Add(new StringContent(value.ToString()), $"CreateAdCarDto.{property.Name}");
                }
            }
        }

        foreach (var photo in createAdDto.Photos)
        {
            var photoContent = new StreamContent(photo.OpenReadStream());
            content.Add(photoContent, "Photos", photo.FileName);
        }

        return content;
    }
}