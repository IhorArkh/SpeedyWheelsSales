using System.Security.Claims;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.EmailOrUserName) ?? 
                   await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.EmailOrUserName);
        
        if (user is null)
            return Unauthorized();

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (result)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
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
                PhotoUrl = user.PhotoUrl
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
            Email = registerDto.Email
        };
        
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            return new UserDto()
            {
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
            };
        }

        return BadRequest(result.Errors);
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }
    
}