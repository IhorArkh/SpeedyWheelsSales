using System.Security.Claims;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

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