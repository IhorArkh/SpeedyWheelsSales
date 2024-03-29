using AutoFixture;
using AutoMapper;
using FluentAssertions;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdDetails;

namespace SpeedyWheelsSales.Tests.Ad.Queries;

public class GetAdDetailsQueryHandlerTests
{
    private const string ContextName = "DbForGetAdDetailsQueryHandler";

    [Fact]
    public async Task Handler_ShouldSuccessfullyReturnAdDetailsDto_WhenAdExists()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ad = fixture.Create<Domain.Entities.Ad>();

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
    public async Task Handler_ShouldReturnSuccessWithNullValue_WhenAdDoesNotExists()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ad = fixture.Create<Domain.Entities.Ad>();

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