using System.Text.Json.Serialization;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Core.Behaviors;
using SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd;
using SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Commands.UpdateAd;
using SpeedyWheelsSales.Application.Features.Ad.Commands.UpdateAd.DTOs;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList;
using SpeedyWheelsSales.Infrastructure;
using SpeedyWheelsSales.Infrastructure.Data;
using SpeedyWheelsSales.WebAPI;
using SpeedyWheelsSales.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(opt => { opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Web Api base address not found.");
builder.Services.AddDatabase(connectionString);
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(GetAdListQueryHandler).Assembly);
    config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
});

builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
builder.Services.AddCors(x => x.AddPolicy("CorsPolicy", policy =>
{
    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    //TODO Need to add specific origin in future.
}));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
builder.Services.AddScoped<IValidator<CreateAdDto>, CreateAdCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateAdDto>, UpdateAdCommandValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseMiddleware<RequestContextLoggingMiddleware>();
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetService<UserManager<AppUser>>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context, userManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();