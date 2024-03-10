using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Ad.Queries.GetAdDetails;
using SpeedyWheelsSales.Application.Ad.Queries.GetAdDetails.DTOs;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Tests.Ad;

public class GetAdDetailsQueryTests
{
    private async Task<DataContext> GetDbContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "DbForGetAdDetailsQuery").Options;

        var databaseContext = new DataContext(options);
        await databaseContext.Database.EnsureDeletedAsync();
        await databaseContext.Database.EnsureCreatedAsync();

        return databaseContext;
    }

    [Fact]
    public async Task Handle_ShouldSuccessfullyReturnAdDetailsDto_WhenAdExists()
    {
        //Arrange
        var context = await GetDbContext();
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ad = fixture.Create<Domain.Ad>();

        context.Ads.Add(ad);
        await context.SaveChangesAsync();

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var query = new GetAdDetailsQuery { Id = ad.Id };
        var handler = new GetAdDetailsQueryHandler(context, mapper);

        //Act

        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(ad, opt => opt.ExcludingMissingMembers());
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessWithNullValue_WhenAdDoesNotExists()
    {
        //Arrange
        var context = await GetDbContext();
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ad = fixture.Create<Domain.Ad>();

        context.Ads.Add(ad);
        await context.SaveChangesAsync();

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        Random random = new Random();

        var query = new GetAdDetailsQuery { Id = random.Next() };
        var handler = new GetAdDetailsQueryHandler(context, mapper);

        //Act

        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeNull();
        result.Error.Should().BeNull();
        result.IsEmpty.Should().BeFalse();
    }
}