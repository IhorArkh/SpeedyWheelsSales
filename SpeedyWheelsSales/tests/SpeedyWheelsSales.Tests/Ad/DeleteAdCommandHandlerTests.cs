using AutoFixture;
using Domain.Interfaces;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpeedyWheelsSales.Application.Ad.Commands.DeleteAd;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Tests.Ad;

public class DeleteAdCommandHandlerTests
{
    private async Task<DataContext> GetDbContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "DbForDeleteAdCommandHandler").Options;

        var databaseContext = new DataContext(options);
        await databaseContext.Database.EnsureDeletedAsync();
        await databaseContext.Database.EnsureCreatedAsync();

        return databaseContext;
    }

    [Fact]
    public async Task Handle_ShouldDeleteAd_WhenAdExistsAndUserIsCreator()
    {
        //Arrange
        var context = await GetDbContext();
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var ad = fixture.Create<Domain.Entities.Ad>();

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
        adAfterDeletion.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyResult_WhenAdDoesNotExists()
    {
        //Arrange
        var context = await GetDbContext();

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
    public async Task Handle_ShouldReturnFailureResult_WhenAdExistsAndUserIsNotCreator()
    {
        //Arrange
        var context = await GetDbContext();
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