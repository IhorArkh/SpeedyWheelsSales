using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

namespace SpeedyWheelsSales.IdentityServer;

public class ProfileService : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var username = context.Subject.FindFirst("username").Value;

        if (!string.IsNullOrEmpty(username))
        {
            var claims = new List<Claim>
            {
                new Claim("username", username)
            };

            context.IssuedClaims.AddRange(claims);
        }
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;
    }
}