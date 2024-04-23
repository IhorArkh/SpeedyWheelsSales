using Domain.Entities;
using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SpeedyWheelsSales.IdentityServer;
using SpeedyWheelsSales.Infrastructure.Data;
using Log = SpeedyWheelsSales.IdentityServer.Pages.Log;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();
Config.SetConfiguration(configuration);

var connString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                 throw new InvalidOperationException("Connection string not found.");

var migrationsAssembly = typeof(Log).Assembly.GetName().Name;

builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlServer(connString, sqlOpt =>
        sqlOpt.MigrationsAssembly(migrationsAssembly));
});

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>();

builder.Services.AddTransient<IProfileService, ProfileService>();
builder.Services.AddRazorPages();
builder.Services.AddIdentityServer(opt =>
    {
        opt.Events.RaiseErrorEvents = true;
        opt.Events.RaiseInformationEvents = true;
        opt.Events.RaiseFailureEvents = true;
        opt.Events.RaiseSuccessEvents = true;

        opt.EmitStaticAudienceClaim = true;
    })
    .AddConfigurationStore(opt => opt.ConfigureDbContext = b =>
        b.UseSqlServer(connString, options => options.MigrationsAssembly(migrationsAssembly)))
    .AddOperationalStore(opt => opt.ConfigureDbContext = b =>
        b.UseSqlServer(connString, options => options.MigrationsAssembly(migrationsAssembly)))
    .AddAspNetIdentity<AppUser>()
    .AddProfileService<ProfileService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie()
    .AddGoogle("Google", opt =>
    {
        var googleAuth = builder.Configuration.GetSection("Authentication:Google");

        opt.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
        opt.ClientId = googleAuth["ClientId"];
        opt.ClientSecret = googleAuth["ClientSecret"];
    });

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console(
        outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

app.UseIdentityServer();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages().RequireAuthorization();

if (args.Contains("/seed"))
{
    Serilog.Log.Information("Seeding database...");
    SeedData.EnsureSeedData(app);
    Serilog.Log.Information("Done seeding database. Exiting.");
    return;
}

app.Run();