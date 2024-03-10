using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Ad.Queries.GetAdList;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Tests.Ad;

public class GetAdListQueryTests
{
    private async Task<DataContext> GetDbContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "DbForGetAdListQuery").Options;

        var databaseContext = new DataContext(options);
        await databaseContext.Database.EnsureDeletedAsync();
        await databaseContext.Database.EnsureCreatedAsync();

        return databaseContext;
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResultWithAdDtos_WhenAdsExist()
    {
        //Arrange
        var context = await GetDbContext();
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ads = fixture.CreateMany<Domain.Ad>().ToList();

        context.Ads.AddRange(ads);
        await context.SaveChangesAsync();

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var query = new GetAdListQuery();
        var handler = new GetAdListQueryHandler(context, mapper);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(ads, opt => opt.ExcludingMissingMembers());
        result.Value.Should().HaveCount(ads.Count());
        result.Value.FirstOrDefault().Id.Should().Be(ads.FirstOrDefault().Id);
        result.Error.Should().BeNull();
    }
}