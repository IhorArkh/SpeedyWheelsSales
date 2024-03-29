using AutoFixture;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;
using SpeedyWheelsSales.Application.Interfaces;

namespace SpeedyWheelsSales.Tests.Ad.Queries;

public class GetAdListQueryHandlerTests
{
    private const string ContextName = "DbForGetAdListQueryHandler";

    [Fact]
    public async Task Handler_ShouldReturnSuccessResultWithPagedAdDtos_WhenAdsExistAndPaginationParamsNotProvided()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ads = fixture.CreateMany<Domain.Entities.Ad>(3).ToList();

        context.Ads.AddRange(ads);
        await context.SaveChangesAsync();

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var adsQuery = context.Ads
            .Include(x => x.Car)
            .Include(x => x.Photos)
            .ProjectTo<AdListDto>(mapper.ConfigurationProvider)
            .AsQueryable();

        var adParams = new AdParams();

        var mockSortingService = new Mock<ISortingService>();
        mockSortingService.Setup(x => x.SortAds(It.IsAny<IQueryable<AdListDto>>(), adParams))
            .Returns(adsQuery.OrderByDescending(x => x.CreatedAt).AsQueryable);

        var mockFilteringService = new Mock<IFilteringService>();
        mockFilteringService.Setup(x => x.FilterAds(It.IsAny<IQueryable<AdListDto>>(), adParams))
            .Returns(adsQuery.OrderByDescending(x => x.CreatedAt).AsQueryable);

        var query = new GetAdListQuery { AdParams = adParams };
        var handler =
            new GetAdListQueryHandler(context, mapper, mockFilteringService.Object, mockSortingService.Object);

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
    public async Task Handler_ShouldReturnSuccessResultWithPagedAdDtos_WhenAdsExistAndPaginationParamsProvided()
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

        var adsQuery = context.Ads
            .Include(x => x.Car)
            .Include(x => x.Photos)
            .ProjectTo<AdListDto>(mapper.ConfigurationProvider)
            .AsQueryable();

        var adParams = new AdParams { PageNumber = 2, PageSize = 2 };

        var mockSortingService = new Mock<ISortingService>();
        mockSortingService.Setup(x => x.SortAds(It.IsAny<IQueryable<AdListDto>>(), adParams))
            .Returns(adsQuery.OrderByDescending(x => x.CreatedAt).AsQueryable);

        var mockFilteringService = new Mock<IFilteringService>();
        mockFilteringService.Setup(x => x.FilterAds(It.IsAny<IQueryable<AdListDto>>(), adParams))
            .Returns(adsQuery.OrderByDescending(x => x.CreatedAt).AsQueryable);

        var query = new GetAdListQuery { AdParams = adParams };
        var handler =
            new GetAdListQueryHandler(context, mapper, mockFilteringService.Object, mockSortingService.Object);

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