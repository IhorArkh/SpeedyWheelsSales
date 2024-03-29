using AutoFixture;
using FluentAssertions;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;
using SpeedyWheelsSales.Application.Services;

namespace SpeedyWheelsSales.Tests.Services;

public class FilteringServiceTests
{
    [Fact]
    public void FilteringService_ShouldReturnQueryWithoutChanges_WhenAdParamsNotProvided()
    {
        //Arrange
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ads = fixture.CreateMany<AdListDto>(4).AsQueryable();

        var adParams = new AdParams();
        var filteringService = new FilteringService();

        //Act
        var filteredAds = filteringService.FilterAds(ads, adParams).ToList();
        var correctResult = ads.ToList();

        //Assert
        filteredAds.Should().BeEquivalentTo(correctResult);
    }

    [Fact]
    public void FilteringService_ShouldReturnFilteredQuery_WhenFilteringParamsProvided()
    {
        //Arrange
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ads = fixture.CreateMany<AdListDto>(4).AsQueryable();
        var listAds = ads.ToList();

        var adParams = new AdParams()
        {
            Brand = listAds[1].CarDto.Brand,
            City = listAds[1].City
        };
        var filteringService = new FilteringService();

        //Act
        var filteredAds = filteringService.FilterAds(ads, adParams).ToList();
        var correctResult = ads.Where(x => x.City == listAds[1].City &&
                                           x.CarDto.Brand == listAds[1].CarDto.Brand).ToList();

        //Assert
        filteredAds.Should().BeEquivalentTo(correctResult);
    }
}