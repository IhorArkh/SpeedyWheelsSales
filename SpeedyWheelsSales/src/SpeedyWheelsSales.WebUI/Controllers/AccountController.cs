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
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        // var user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == loginViewModel.PhoneOrUserName) ??
        //            await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginViewModel.PhoneOrUserName);
        //
        // if (user is null)
        //     return Unauthorized();
        //
        // var result = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
        //
        // if (result)
        // {
        //     var claims = new List<Claim>
        //     {
        //         new Claim(ClaimTypes.Name, user.UserName),
        //     };
        //
        //     var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //
        //     await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
        //         new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties()
        //         {
        //             IsPersistent = true
        //         });
        //
        //     return RedirectToAction("ListAds", "Ad");
        // }

        var token = await HttpContext.GetTokenAsync("access_token");


        return View();
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
        return RedirectToAction("ListAds", "Ad");
    }
}