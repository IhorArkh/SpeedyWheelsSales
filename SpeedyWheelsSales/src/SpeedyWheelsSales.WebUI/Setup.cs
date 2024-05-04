using Domain.Entities;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.WebUI;

public static class Setup
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookie";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("cookie")
            .AddOpenIdConnect("oidc", opt =>
            {
                opt.Authority = config["InteractiveServiceSettings:AuthorityUrl"];
                opt.ClientId = config["InteractiveServiceSettings:ClientId"];
                opt.ClientSecret = config["InteractiveServiceSettings:ClientSecret"];
                opt.Scope.Add(config["InteractiveServiceSettings:Scopes:0"]);

                opt.GetClaimsFromUserInfoEndpoint = true;
                opt.ResponseType = "code";
                opt.UsePkce = true;
                opt.ResponseMode = "query";
                opt.SaveTokens = true;
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