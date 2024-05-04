using Domain.Entities;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.WebAPI;

public static class Setup
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.Authority = config["IdentityServerConfig:Authority"];
                opt.Audience = config["IdentityServerConfig:Audience"];

                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidTypes = new[] { "at+jwt" },
                    RequireSignedTokens = false
                };

                opt.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var accessToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                        var introspectionEndpoint = config["IdentityServerConfig:IntrospectionEndpoint"];
                        var clientId = config["IdentityServerConfig:ClientId"];
                        var clientSecret = config["IdentityServerConfig:ClientSecret"];

                        using (var client = new HttpClient())
                        {
                            var request = new TokenIntrospectionRequest
                            {
                                Address = introspectionEndpoint,
                                ClientId = clientId,
                                ClientSecret = clientSecret,
                                Token = accessToken,
                            };

                            var response = await client.IntrospectTokenAsync(request);

                            if (!response.IsActive)
                            {
                                context.Fail("Token is not valid.");
                                throw new Exception("Authentication failed.");
                            }
                        }
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
                        logger.LogError("Authentication failed: " + context.Exception.Message);
                        return Task.CompletedTask;
                    }
                };
            });
        services.AddAuthorization();
        services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<DataContext>();

        return services;
    }
}