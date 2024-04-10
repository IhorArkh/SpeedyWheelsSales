using AutoFixture;
using Domain.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpeedyWheelsSales.Application.Features.Ad.Commands.ToggleFavouriteAd;
using SpeedyWheelsSales.Application.Interfaces;

namespace SpeedyWheelsSales.Tests.Features.Ad.Commands;

public class ToggleFavouriteAdCommandHandlerTests
{
    private const string ContextName = "DbForToggleFavouriteAdCommandHandler";

    [Fact]
    public async Task Handler_ShouldAddAdToFavourites_WhenItWasNotAddedYet()
    {
        // Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ad = fixture.Create<Domain.Entities.Ad>();
        ad.AppUser = null;
        ad.FavouriteAds = new List<FavouriteAd>();

        var user = fixture.Create<AppUser>();
        user.FavouriteAds = new List<FavouriteAd>();
        user.Ads = new List<Domain.Entities.Ad>();

        context.Ads.Add(ad);
        context.AppUsers.Add(user);
        await context.SaveChangesAsync();

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(user.UserName);

        var command = new ToggleFavouriteAdCommand() { AdId = ad.Id };
        var handler = new ToggleFavouriteAdCommandHandler(context, userAccessorMock.Object);

        var favAdsCountBeforeAdding = user.FavouriteAds.Count;

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        var userFromDb = await context.AppUsers
            .Include(x => x.FavouriteAds)
            .FirstOrDefaultAsync(x => x.Id == user.Id);

        var userFavAd = userFromDb.FavouriteAds.SingleOrDefault(x => x.AdId == ad.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValidationErrors.Should().BeNullOrEmpty();
        result.IsEmpty.Should().BeFalse();
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeNullOrEmpty();

        userFromDb.FavouriteAds.Count.Should().Be(favAdsCountBeforeAdding + 1);
        userFavAd.Ad.Should().BeEquivalentTo(ad);
    }

    [Fact]
    public async Task Handler_ShouldRemoveFromFavourites_WhenItWasAlreadyAdded()
    {
        // Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var user = fixture.Create<AppUser>();

        context.AppUsers.Add(user);
        await context.SaveChangesAsync();

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(user.UserName);

        var favAd = user.FavouriteAds.FirstOrDefault();
        var favAdsCountBeforeRemoving = user.FavouriteAds.Count;

        var command = new ToggleFavouriteAdCommand() { AdId = favAd.AdId };
        var handler = new ToggleFavouriteAdCommandHandler(context, userAccessorMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        var userFromDb = await context.AppUsers
            .Include(x => x.FavouriteAds)
            .FirstOrDefaultAsync(x => x.Id == user.Id);

        var favAdAfterRemoving = userFromDb.FavouriteAds
            .FirstOrDefault(x => x.AdId == favAd.AdId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValidationErrors.Should().BeNullOrEmpty();
        result.IsEmpty.Should().BeFalse();
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeNullOrEmpty();

        userFromDb.FavouriteAds.Count.Should().Be(favAdsCountBeforeRemoving - 1);
        favAdAfterRemoving.Should().BeNull();
    }

    [Fact]
    public async Task Handler_ShouldReturnEmptyResult_WhenAdNotFoundInDb()
    {
        // Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var user = fixture.Create<AppUser>();
        user.FavouriteAds = new List<FavouriteAd>();
        user.Ads = new List<Domain.Entities.Ad>();

        context.AppUsers.Add(user);
        await context.SaveChangesAsync();

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(user.UserName);

        var command = new ToggleFavouriteAdCommand() { AdId = -1 }; // wrong id
        var handler = new ToggleFavouriteAdCommandHandler(context, userAccessorMock.Object);

        var favAdsCountBeforeAdding = user.FavouriteAds.Count;

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        var userFromDb = await context.AppUsers
            .Include(x => x.FavouriteAds)
            .FirstOrDefaultAsync(x => x.Id == user.Id);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().BeNullOrEmpty();
        result.IsEmpty.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeNullOrEmpty();

        userFromDb.FavouriteAds.Count.Should().Be(favAdsCountBeforeAdding);
    }

    [Fact]
    public async Task Handler_ShouldReturnEmptyResult_WhenUserNotFoundInDb()
    {
        // Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ad = fixture.Create<Domain.Entities.Ad>();
        ad.AppUser = null;
        ad.FavouriteAds = new List<FavouriteAd>();

        var user = fixture.Create<AppUser>();
        user.FavouriteAds = new List<FavouriteAd>();
        user.Ads = new List<Domain.Entities.Ad>();

        context.Ads.Add(ad);
        context.AppUsers.Add(user);
        await context.SaveChangesAsync();

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns("wrongUsername");

        var command = new ToggleFavouriteAdCommand() { AdId = ad.Id };
        var handler = new ToggleFavouriteAdCommandHandler(context, userAccessorMock.Object);

        var favAdsCountBeforeAdding = user.FavouriteAds.Count;

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        var userFromDb = await context.AppUsers
            .Include(x => x.FavouriteAds)
            .FirstOrDefaultAsync(x => x.Id == user.Id);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().BeNullOrEmpty();
        result.IsEmpty.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeNullOrEmpty();

        userFromDb.FavouriteAds.Count.Should().Be(favAdsCountBeforeAdding);
    }

    [Fact]
    public async Task Handler_ShouldReturnFailure_WhenUserIsAdCreator()
    {
        // Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var user = fixture.Create<AppUser>();

        context.AppUsers.Add(user);
        await context.SaveChangesAsync();

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(user.UserName);

        var command = new ToggleFavouriteAdCommand() { AdId = user.Ads.FirstOrDefault().Id };
        var handler = new ToggleFavouriteAdCommandHandler(context, userAccessorMock.Object);

        var favAdsCountBeforeAdding = user.FavouriteAds.Count;

        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        var userFromDb = await context.AppUsers
            .Include(x => x.FavouriteAds)
            .FirstOrDefaultAsync(x => x.Id == user.Id);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().BeNullOrEmpty();
        result.IsEmpty.Should().BeFalse();
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().NotBeNullOrEmpty();
        result.Error.Should().Be("You can't add your own add to favourites.");

        userFromDb.FavouriteAds.Count.Should().Be(favAdsCountBeforeAdding);
    }
}