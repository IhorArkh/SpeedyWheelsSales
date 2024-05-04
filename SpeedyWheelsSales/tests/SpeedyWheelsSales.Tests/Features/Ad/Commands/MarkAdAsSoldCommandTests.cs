using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpeedyWheelsSales.Application.Features.Ad.Commands.MarkAdAsSold;
using SpeedyWheelsSales.Application.Interfaces;

namespace SpeedyWheelsSales.Tests.Features.Ad.Commands;

public class MarkAdAsSoldCommandTests
{
    private const string ContextName = "DbForMarkAdAsSoldCommandHandler";

    [Fact]
    public async Task Handler_ShouldMarkAdAsSold_WhenAdExistsAndUserIsCreator()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ad = fixture.Create<Domain.Entities.Ad>();
        ad.IsSold = false;
        ad.SoldAt = null;

        context.Ads.Add(ad);
        await context.SaveChangesAsync();

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(ad.AppUser.UserName);

        var command = new MarkAdAsSoldCommand { Id = ad.Id };
        var handler = new MarkAdAsSoldCommandHandler(context, userAccessorMock.Object);

        //Act

        var result = await handler.Handle(command, CancellationToken.None);

        var adAfterMarking = await context.Ads
            .FirstOrDefaultAsync(x => x.Id == ad.Id);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
        adAfterMarking.IsSold.Should().BeTrue();
        adAfterMarking.SoldAt.Value.Date.Should().Be(DateTime.UtcNow.Date);
        adAfterMarking.Should().BeEquivalentTo(ad, opt =>
            opt.ExcludingMissingMembers().Excluding(x => x.IsSold).Excluding(x => x.SoldAt));
    }

    [Fact]
    public async Task Handler_ShouldReturnEmptyResult_WhenAdDoesNotExists()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);

        var userAccessorMock = new Mock<ICurrentUserAccessor>();

        var random = new Random();

        var command = new MarkAdAsSoldCommand() { Id = random.Next() };
        var handler = new MarkAdAsSoldCommandHandler(context, userAccessorMock.Object);

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

        var command = new MarkAdAsSoldCommand() { Id = ad.Id };
        var handler = new MarkAdAsSoldCommandHandler(context, userAccessorMock.Object);

        //Act

        var result = await handler.Handle(command, CancellationToken.None);

        var adAfterDeletion = await context.Ads.FirstOrDefaultAsync(x => x.Id == ad.Id);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().Be("Users can mark as sold only their own ads.");
        adAfterDeletion.Should().BeEquivalentTo(ad);
    }
}