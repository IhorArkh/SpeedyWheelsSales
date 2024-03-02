using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Ad.Queries.GetAdList;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Infrastructure.Data;
using SpeedyWheelsSales.WebAPI.Extensions;
using SpeedyWheelsSales.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddDataServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddMediatR(x => 
    x.RegisterServicesFromAssembly(typeof(GetAdListQueryHandler).Assembly));
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
builder.Services.AddCors(x => x.AddPolicy("CorsPolicy", policy =>
{
    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    //TODO Need to add specific origin in future.
}));

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();