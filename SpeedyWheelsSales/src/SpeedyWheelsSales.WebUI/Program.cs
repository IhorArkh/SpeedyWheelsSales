using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using SpeedyWheelsSales.Infrastructure.Data;
using SpeedyWheelsSales.WebUI;
using SpeedyWheelsSales.WebUI.Core;
using SpeedyWheelsSales.WebUI.Interfaces;
using SpeedyWheelsSales.WebUI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
builder.Services
    .AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(opt =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("en"),
        new CultureInfo("uk")
    };

    opt.DefaultRequestCulture = new RequestCulture("en");
    opt.SupportedCultures = supportedCultures;
    opt.SupportedUICultures = supportedCultures;
});
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("MyWebApi", x =>
{
    var webApiBaseAddress = builder.Configuration["WebApiBaseAddress"] ??
                            throw new InvalidOperationException("Web Api base address not found.");
    x.BaseAddress = new Uri(webApiBaseAddress);
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string not found.");
builder.Services.AddDatabase(connectionString);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddAutoMapper(typeof(WebUiMappingProfiles).Assembly);
builder.Services.AddScoped<IErrorHandlingService, ErrorHandlingService>();

var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();