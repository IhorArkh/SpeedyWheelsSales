using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SpeedyWheelsSales.Application.Interfaces;

namespace SpeedyWheelsSales.Infrastructure;

public class CurrentUserAccessor : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUsername()
    {
        return _httpContextAccessor.HttpContext.User.FindFirstValue("username");
    }
}