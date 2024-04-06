using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SpeedyWheelsSales.WebUI.Controllers;

public class AccountController : Controller
{
    private readonly string _identityServerBaseUrl;
    private readonly string _webUiBaseUrl;


    public AccountController(IConfiguration configuration)
    {
        _identityServerBaseUrl = configuration["IdentityServerBaseUrl"];
        _webUiBaseUrl = configuration["WebUIBaseUrl"];
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
        var registerUrl = $"{_identityServerBaseUrl}Account/Create/?returnUrl={_webUiBaseUrl}";

        return Redirect(registerUrl);
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