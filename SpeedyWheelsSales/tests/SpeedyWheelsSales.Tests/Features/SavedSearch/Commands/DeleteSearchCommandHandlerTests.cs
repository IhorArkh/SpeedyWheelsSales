using AutoFixture;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpeedyWheelsSales.Application.Features.SavedSearch.Commands.DeleteSearch;

namespace SpeedyWheelsSales.Tests.Features.SavedSearch.Commands;

public class DeleteSearchCommandHandlerTests
{
    private const string ContextName = "DbForDeleteSearchCommandHandler";

    [Fact]
    public async Task Handler_ShouldDeleteSearch_WhenExists()
    {
        //Arrange
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

        var savedSearch = user.SavedSearches.FirstOrDefault();

        var command = new DeleteSearchCommand { Id = savedSearch.Id };
        var handler = new DeleteSearchCommandHandler(context, userAccessorMock.Object);

        var savedSearchesCountBeforeDeletion = user.SavedSearches.Count;

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        var userFromDb = await context.AppUsers
            .Include(x => x.FavouriteAds)
            .FirstOrDefaultAsync(x => x.Id == user.Id);

        var deletedSearch = userFromDb.FavouriteAds.FirstOrDefault(x => x.Id == savedSearch.Id);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.IsEmpty.Should().BeFalse();
        result.Error.Should().BeNullOrEmpty();
        result.Value.Should().Be(Unit.Value);

        deletedSearch.Should().BeNull();
        userFromDb.SavedSearches.Count.Should().Be(savedSearchesCountBeforeDeletion - 1);
    }

    [Fact]
    public async Task Handler_ShouldReturnResultEmpty_WhenUserNotFoundInDb()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns("wrongUsername");

        var command = new DeleteSearchCommand { Id = -1 };
        var handler = new DeleteSearchCommandHandler(context, userAccessorMock.Object);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.IsEmpty.Should().BeTrue();
        result.Error.Should().BeNullOrEmpty();
        result.Value.Should().Be(Unit.Value);
    }

    [Fact]
    public async Task Handler_ShouldReturnFailure_WhenSearchDoesNotExists()
    {
        //Arrange
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

        var command = new DeleteSearchCommand { Id = -1 }; // wrong search Id
        var handler = new DeleteSearchCommandHandler(context, userAccessorMock.Object);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.IsEmpty.Should().BeFalse();
        result.Error.Should().Be("Search does not exist.");
        result.Value.Should().Be(Unit.Value);
    }
}