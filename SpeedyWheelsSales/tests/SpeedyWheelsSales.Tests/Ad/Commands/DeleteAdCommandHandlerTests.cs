using AutoFixture;
using Domain.Interfaces;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpeedyWheelsSales.Application.Features.Ad.Commands.DeleteAd;

namespace SpeedyWheelsSales.Tests.Ad.Commands;

public class DeleteAdCommandHandlerTests
{
    private const string ContextName = "DbForDeleteAdCommandHandler";

    [Fact]
    public async Task Handler_ShouldDeleteAd_WhenAdExistsAndUserIsCreator()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ad = fixture.Create<Domain.Entities.Ad>();
        ad.IsDeleted = false;

        context.Ads.Add(ad);
        await context.SaveChangesAsync();

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(ad.AppUser.UserName);

        var command = new DeleteAdCommand { Id = ad.Id };
        var handler = new DeleteAdCommandHandler(context, userAccessorMock.Object);

        //Act

        var result = await handler.Handle(command, CancellationToken.None);

        var adAfterDeletion = await context.Ads
            .FirstOrDefaultAsync(x => x.Id == ad.Id);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
        adAfterDeletion.IsDeleted.Should().BeTrue();
        adAfterDeletion.Should().BeEquivalentTo(ad, opt =>
            opt.ExcludingMissingMembers().Excluding(x => x.IsDeleted));
    }

    [Fact]
    public async Task Handler_ShouldReturnEmptyResult_WhenAdDoesNotExists()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);

        var userAccessorMock = new Mock<ICurrentUserAccessor>();

        var random = new Random();

        var command = new DeleteAdCommand { Id = random.Next() };
        var handler = new DeleteAdCommandHandler(context, userAccessorMock.Object);

        //Act

        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.IsEmpty.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
    }

    [Fact]
    public async Task Handler_ShouldReturnFailureResult_WhenAdExistsAndUserIsNotCreator()
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

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns("otherUsername");

        var command = new DeleteAdCommand { Id = ad.Id };
        var handler = new DeleteAdCommandHandler(context, userAccessorMock.Object);

        //Act

        var result = await handler.Handle(command, CancellationToken.None);

        var adAfterDeletion = await context.Ads.FirstOrDefaultAsync(x => x.Id == ad.Id);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().Be("Users can delete only their own ads.");
        adAfterDeletion.Should().BeEquivalentTo(ad);
    }
}