using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetFavouriteAds;

namespace SpeedyWheelsSales.Tests.Ad.Queries;

public class GetFavouriteAdsQueryHandlerTests
{
    private const string ContextName = "DbForGetFavouriteAdsQueryHandler";

    [Fact]
    public async Task Handler_ShouldReturnListWithFavouriteAds_WhenExists()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var user = fixture.Create<AppUser>();

        context.AppUsers.Add(user);
        await context.SaveChangesAsync();

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(user.UserName);

        var pagingParams = new PagingParams();

        var query = new GetFavouriteAdsQuery { PagingParams = pagingParams };
        var handler = new GetFavouriteAdsQueryHandler(context, userAccessorMock.Object, mapper);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNullOrEmpty();
        result.ValidationErrors.Should().BeNullOrEmpty();
        result.IsEmpty.Should().BeFalse();
        result.Value.Count.Should().Be(user.FavouriteAds.Count);
        result.Value.Should().BeEquivalentTo(user.FavouriteAds, opt =>
            opt.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Handler_ShouldReturnEmptyResult_WhenUserNotFoundInDb()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns("wrongUsername");

        var pagingParams = new PagingParams();

        var query = new GetFavouriteAdsQuery { PagingParams = pagingParams };
        var handler = new GetFavouriteAdsQueryHandler(context, userAccessorMock.Object, mapper);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeNullOrEmpty();
        result.IsEmpty.Should().BeTrue();
        result.ValidationErrors.Should().BeNullOrEmpty();
        result.Value.Should().BeNullOrEmpty();
    }
}