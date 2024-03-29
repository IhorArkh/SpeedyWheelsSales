using AutoFixture;
using FluentAssertions;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;
using SpeedyWheelsSales.Application.Services;

namespace SpeedyWheelsSales.Tests.Services;

public class SortingServiceTests
{
    [Fact]
    public void SortAds_ShouldReturnAdsSortedByCreatedDateDesc_WhenParamsNotProvided()
    {
        //Arrange
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ads = fixture.CreateMany<AdListDto>(4).AsQueryable();

        var adParams = new AdParams();
        var sortingService = new SortingService();

        //Act
        var sortedAds = sortingService.SortAds(ads, adParams).ToList();
        var correctResult = ads.OrderByDescending(x => x.CreatedAt).ToList();

        //Assert
        sortedAds.Should().BeEquivalentTo(correctResult);
    }

    [Fact]
    public void SortAds_ShouldReturnAdsSortedByPriceAscending_WhenSortingParamsProvidedInAnyCase()
    {
        //Arrange
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ads = fixture.CreateMany<AdListDto>(4).AsQueryable();

        var adParams = new AdParams
        {
            SortColumn = "pRiCe",
            SortOrder = "asc"
        };
        var sortingService = new SortingService();

        //Act
        var sortedAds = sortingService.SortAds(ads, adParams).ToList();
        var correctResult = ads.OrderBy(x => x.CarDto.Price).ToList();

        //Assert
        sortedAds.Should().BeEquivalentTo(correctResult);
    }

    [Fact]
    public void SortAds_ShouldReturnAdsSortedByCreatedDateDesc_WhenWrongParamsProvided()
    {
        //Arrange
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ads = fixture.CreateMany<AdListDto>(4).AsQueryable();

        var adParams = new AdParams
        {
            SortColumn = "wrongSortColumn",
            SortOrder = "wrongSortOrder"
        };
        var sortingService = new SortingService();

        //Act
        var sortedAds = sortingService.SortAds(ads, adParams).ToList();
        var correctResult = ads.OrderByDescending(x => x.CreatedAt).ToList();

        //Assert
        sortedAds.Should().BeEquivalentTo(correctResult);
    }

    [Fact]
    public void SortAds_ShouldReturnAdsSortedByDescending_WhenNoSortingOrderParamProvided()
    {
        //Arrange
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ads = fixture.CreateMany<AdListDto>(4).AsQueryable();

        var adParams = new AdParams
        {
            SortColumn = "ManufactureDATE",
        };
        var sortingService = new SortingService();

        //Act
        var sortedAds = sortingService.SortAds(ads, adParams).ToList();
        var correctResult = ads.OrderByDescending(x => x.CarDto.ManufactureDate).ToList();

        //Assert
        sortedAds.Should().BeEquivalentTo(correctResult);
    }
}