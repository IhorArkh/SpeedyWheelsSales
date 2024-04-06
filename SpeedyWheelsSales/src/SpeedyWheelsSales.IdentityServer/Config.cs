using Duende.IdentityServer.Models;

namespace SpeedyWheelsSales.IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = "customProfile",
                UserClaims = new List<string> { "username" }
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new[]
        {
            new ApiScope("speedywheelssalesapi.read"),
            new ApiScope("speedywheelssalesapi.write"),
        };

    public static IEnumerable<ApiResource> ApiResources => new[]
    {
        new ApiResource("speedywheelssalesapi")
        {
            Scopes = new List<string> { "speedywheelssalesapi.read", "speedywheelssalesapi.write" },
            ApiSecrets = new List<Secret>
                { new Secret("ScopeSecret".Sha256()) }, // TODO secrets should be in appsettings file
            UserClaims = new List<string> { "username" }
        }
    };

    public static IEnumerable<Client> Clients =>
        new[]
        {
            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "speedywheelssaleswebui",
                ClientSecrets = { new Secret("SuperSecretPasswordSuperSecretPasswordSuperSecretPassword".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:5003/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:5003/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:5003/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                AlwaysSendClientClaims = true,
                AllowedScopes = { "openid", "profile", "speedywheelssalesapi.read", "customProfile" },
                RequirePkce = true,
                AllowPlainTextPkce = false,
                RequireConsent = false
            }
        };
}