using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList;

namespace SpeedyWheelsSales.Tests.Ad.Queries;

public class GetAdListQueryHandlerTests // TODO Need to update tests for GetAdList
{
    private const string ContextName = "DbForGetAdListQueryHandler";

    [Fact]
    public async Task Handle_ShouldReturnSuccessResultWithPagedAdDtos_WhenAdsExistAndPaginationParamsNotProvided()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ads = fixture.CreateMany<Domain.Entities.Ad>().ToList();

        context.Ads.AddRange(ads);
        await context.SaveChangesAsync();

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var adParams = new AdParams();

        var query = new GetAdListQuery { AdParams = adParams };
        var handler = new GetAdListQueryHandler(context, mapper);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);
        var adsFromDb = await context.Ads
            .OrderByDescending(x => x.CreatedAt)
            .Take(10)
            .ToListAsync();

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(adsFromDb, opt => opt.ExcludingMissingMembers());
        result.Value.Should().HaveCount(adsFromDb.Count());
        result.Value.FirstOrDefault().Id.Should().Be(adsFromDb.FirstOrDefault().Id);
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResultWithPagedAdDtos_WhenAdsExistAndPaginationParamsProvided()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ads = fixture.CreateMany<Domain.Entities.Ad>(6).ToList();

        context.Ads.AddRange(ads);
        await context.SaveChangesAsync();

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var adParams = new AdParams { PageNumber = 2, PageSize = 2 };

        var query = new GetAdListQuery { AdParams = adParams };
        var handler = new GetAdListQueryHandler(context, mapper);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);
        var adsFromDb = await context.Ads
            .OrderByDescending(x => x.CreatedAt)
            .Skip(2)
            .Take(2)
            .ToListAsync();

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(adsFromDb, opt => opt.ExcludingMissingMembers());
        result.Value.Should().HaveCount(adsFromDb.Count());
        result.Value.FirstOrDefault().Id.Should().Be(adsFromDb.FirstOrDefault().Id);
        result.Error.Should().BeNull();
    }
}