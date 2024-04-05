using System.Security.Claims;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.WebUI.Models;

namespace SpeedyWheelsSales.WebUI.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly HttpClient _httpClient;

    public AccountController(UserManager<AppUser> userManager, IHttpClientFactory httpClientFactory)
    {
        _userManager = userManager;
        _httpClient = httpClientFactory.CreateClient();
    }

    [Authorize]
    [HttpGet]
    public IActionResult Login()
    {
        return RedirectToAction("ListAds", "Ad");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        var user = new AppUser()
        {
            Name = registerViewModel.Name,
            UserName = registerViewModel.UserName,
            Email = registerViewModel.Email,
            PhoneNumber = registerViewModel.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, registerViewModel.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties()
            {
                IsPersistent = true
            });

        return RedirectToAction("ListAds", "Ad");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LogoutConfirmed()
    {
        await HttpContext.SignOutAsync();
        Response.Cookies.Delete(".AspNetCore.Identity.Application");
        Response.Cookies.Delete(".AspNetCore.Antiforgery");
        Response.Cookies.Delete(".AspNetCore.cookie");
        return RedirectToAction("ListAds", "Ad");
    }
}