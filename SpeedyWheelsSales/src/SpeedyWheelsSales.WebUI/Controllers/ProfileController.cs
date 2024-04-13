using System.Diagnostics;
using System.Text;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SpeedyWheelsSales.Application.Features.Profile.Commands.UpdateUserProfile.DTOs;
using SpeedyWheelsSales.Application.Features.Profile.Queries.GetUserProfile.DTOs;
using SpeedyWheelsSales.WebUI.Models;

namespace SpeedyWheelsSales.WebUI.Controllers;

public class ProfileController : Controller
{
    private readonly HttpClient _httpClient;

    public ProfileController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("MyWebApi");
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile(string? username)
    {
        var path = "Profile";

        if (username == null)
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            if (token != null)
                _httpClient.SetBearerToken(token);
        }
        else
        {
            path = $"Profile?username={username}";
        }

        var response = await _httpClient.GetAsync($"{path}");

        if (!response.IsSuccessStatusCode)
        {
            var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View("Error", errorViewModel);
        }

        var responseData = await response.Content.ReadAsStringAsync();

        var userProfile = JsonConvert.DeserializeObject<UserProfileDto>(responseData);

        var userProfileVm = new UserProfileViewModel
        {
            IsOwner = username == null,
            UserProfileDto = userProfile
        };

        return View(userProfileVm);
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetUpdateProfileView()
    {
        return View(); // TODO need to pass previous values to view
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdateProfile(UpdateUserProfileDto updateUserProfileDto)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        if (token != null)
            _httpClient.SetBearerToken(token);

        var jsonContent = JsonConvert.SerializeObject(updateUserProfileDto);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync("Profile", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View("Error", errorViewModel);
        }

        return RedirectToAction("GetProfile");
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetChangePhotoView()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ChangePhoto(IFormFile photo)
    {
        if (photo is null)
            return BadRequest();

        var token = await HttpContext.GetTokenAsync("access_token");

        if (token != null)
            _httpClient.SetBearerToken(token);

        var content = new MultipartFormDataContent();
        var photoContent = new StreamContent(photo.OpenReadStream());
        content.Add(photoContent, "photo", photo.FileName);

        var response = await _httpClient.PutAsync("Profile/changePhoto", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View("Error", errorViewModel);
        }

        return RedirectToAction("GetProfile");
    }
}