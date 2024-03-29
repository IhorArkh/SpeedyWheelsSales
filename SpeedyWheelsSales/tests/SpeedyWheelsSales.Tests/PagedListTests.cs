using AutoFixture;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList.DTOs;

namespace SpeedyWheelsSales.Tests;

public class PagedListTests
{
    private const string ContextName = "DbForPagedList";

    [Fact]
    public async Task CreateAsync_ShouldReturnCorrectListOfItems()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);

        var fixture = new Fixture();
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var ads = fixture.CreateMany<Ad>(10);

        context.Ads.AddRange(ads);
        await context.SaveChangesAsync();

        var adsQuery = context.Ads.ProjectTo<AdListDto>(mapper.ConfigurationProvider).AsQueryable();

        //Act
        var result = await PagedList<AdListDto>.CreateAsync(adsQuery, 2, 5);
        var correctResult = await adsQuery.Skip(5).Take(5).ToListAsync();

        //Assert
        result.Should().BeEquivalentTo(correctResult);

        result.TotalCount.Should().Be(10);
        result.PageSize.Should().Be(5);
        result.CurrentPage.Should().Be(2);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnEmptyCollection_WhenPageNumberGreaterThanLastPageNumber()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);

        var fixture = new Fixture();
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var ads = fixture.CreateMany<Ad>(15);

        context.Ads.AddRange(ads);
        await context.SaveChangesAsync();

        var adsQuery = context.Ads.ProjectTo<AdListDto>(mapper.ConfigurationProvider).AsQueryable();

        //Act
        var result = await PagedList<AdListDto>.CreateAsync(adsQuery, 4, 5);

        //Assert
        result.Should().BeEmpty();

        result.TotalCount.Should().Be(15);
        result.PageSize.Should().Be(5);
        result.CurrentPage.Should().Be(4);
    }
}