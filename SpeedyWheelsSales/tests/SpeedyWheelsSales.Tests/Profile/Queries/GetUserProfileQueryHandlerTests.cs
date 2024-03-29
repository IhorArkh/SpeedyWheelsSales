using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Queries.GetAdList;
using SpeedyWheelsSales.Application.Features.Profile.Queries.GetUserProfile;

namespace SpeedyWheelsSales.Tests.Profile.Queries;

public class GetUserProfileQueryHandlerTests
{
    private const string ContextName = "DbForGetCurrUserProfileQueryHandler";

    [Fact]
    public async Task Handler_ShouldReturnSuccessResultWithCurrUserProfileDto_WhenUsernameNotProvided()
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

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(user.UserName);

        var query = new GetUserProfileQuery();
        var handler = new GetUserProfileQueryHandler(context, userAccessorMock.Object, mapper);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);
        var userFromDb = await context.AppUsers.SingleOrDefaultAsync();

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(user, opt => opt.ExcludingMissingMembers());
        result.IsEmpty.Should().BeFalse();
        result.Error.Should().BeNullOrEmpty();
        userFromDb.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task Handler_ShouldReturnSuccessResultWithUserProfileDto_WhenUsernameProvided()
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

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns("OtherUsername");

        var query = new GetUserProfileQuery { Username = user.UserName };
        var handler = new GetUserProfileQueryHandler(context, userAccessorMock.Object, mapper);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);
        var userFromDb = await context.AppUsers.SingleOrDefaultAsync();

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(user, opt => opt.ExcludingMissingMembers());
        result.IsEmpty.Should().BeFalse();
        result.Error.Should().BeNullOrEmpty();
        userFromDb.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task Handler_ShouldReturnEmptyResult_WhenUsernameNotProvidedAndCantFigureOutCurrUserUsername()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(() => null);

        var query = new GetUserProfileQuery();
        var handler = new GetUserProfileQueryHandler(context, userAccessorMock.Object, mapper);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);
        var users = await context.AppUsers.FirstOrDefaultAsync();

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.IsEmpty.Should().BeTrue();
        result.Error.Should().BeNullOrEmpty();
        users.Should().BeNull();
    }

    [Fact]
    public async Task Handler_ShouldReturnEmptyResult_UserWithCurrUsernameWasNotFoundInDb()
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

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns("otherUsername");

        var query = new GetUserProfileQuery();
        var handler = new GetUserProfileQueryHandler(context, userAccessorMock.Object, mapper);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);
        var userFromDb = await context.AppUsers.SingleOrDefaultAsync();

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().BeNull();
        result.IsEmpty.Should().BeTrue();
        result.Error.Should().BeNullOrEmpty();
        userFromDb.Should().BeEquivalentTo(user);
    }
}