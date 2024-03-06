﻿using System.Security.Claims;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.WebAPI.DTOs;

namespace SpeedyWheelsSales.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;

    public AccountController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == loginDto.PhoneOrUserName) ??
                   await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.PhoneOrUserName);

        if (user is null)
            return Unauthorized();

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (result)
        {
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

            return new UserDto()
            {
                Name = user.Name,
                UserName = user.UserName,
                PhotoUrl = user.PhotoUrl,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        return Unauthorized();
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        var user = new AppUser()
        {
            Name = registerDto.Name,
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

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

        return new UserDto()
        {
            Name = user.Name,
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }
}