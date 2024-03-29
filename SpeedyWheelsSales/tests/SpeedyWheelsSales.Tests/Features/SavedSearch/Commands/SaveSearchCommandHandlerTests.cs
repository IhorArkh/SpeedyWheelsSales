using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.SavedSearch.Commands.SaveSearch;

namespace SpeedyWheelsSales.Tests.Features.SavedSearch.Commands;

public class SaveSearchCommandHandlerTests
{
    private const string ContextName = "DbForSaveSearchCommandHandler";

    [Fact]
    public async Task Handler_ShouldSaveSearch_WhenNotAddedYet()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var httpRequestMock = new Mock<HttpRequest>();
        var queryStringValue = "queryString";
        var queryString = new QueryString($"?{queryStringValue}");
        httpRequestMock.SetupGet(r => r.QueryString).Returns(queryString);
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupGet(c => c.Request).Returns(httpRequestMock.Object);
        httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(httpContextMock.Object);

        var user = fixture.Create<AppUser>();
        user.SavedSearches = new List<Domain.Entities.SavedSearch>();

        context.AppUsers.Add(user);
        await context.SaveChangesAsync();

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(user.UserName);

        var saveSearchParams = fixture.Create<SaveSearchParams>();

        var command = new SaveSearchCommand { SaveSearchParams = saveSearchParams };
        var handler =
            new SaveSearchCommandHandler(context, httpContextAccessorMock.Object, userAccessorMock.Object, mapper);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        var savedSearch = await context.SavedSearches.FirstOrDefaultAsync(x => x.AppUserId == user.Id);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNullOrEmpty();
        result.IsEmpty.Should().BeFalse();
        result.Value.Should().Be(Unit.Value);

        savedSearch.Should().BeEquivalentTo(saveSearchParams, opt =>
            opt.ExcludingMissingMembers());
        savedSearch.QueryString.Should().Be($"?{queryStringValue}");
    }

    [Fact]
    public async Task Handler_ShouldReturnFailure_WhenParamsNotProvided()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var httpRequestMock = new Mock<HttpRequest>();
        var queryString = new QueryString("");
        httpRequestMock.SetupGet(r => r.QueryString).Returns(queryString);
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupGet(c => c.Request).Returns(httpRequestMock.Object);
        httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(httpContextMock.Object);

        var userAccessorMock = new Mock<ICurrentUserAccessor>();

        var saveSearchParams = new SaveSearchParams();

        var command = new SaveSearchCommand { SaveSearchParams = saveSearchParams };
        var handler =
            new SaveSearchCommandHandler(context, httpContextAccessorMock.Object, userAccessorMock.Object, mapper);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Can't save search without parameters.");
        result.IsEmpty.Should().BeFalse();
        result.Value.Should().Be(Unit.Value);
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

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var httpRequestMock = new Mock<HttpRequest>();
        var queryStringValue = "queryString";
        var queryString = new QueryString($"?{queryStringValue}");
        httpRequestMock.SetupGet(r => r.QueryString).Returns(queryString);
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupGet(c => c.Request).Returns(httpRequestMock.Object);
        httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(httpContextMock.Object);

        var user = fixture.Create<AppUser>();
        user.SavedSearches = new List<Domain.Entities.SavedSearch>();

        context.AppUsers.Add(user);
        await context.SaveChangesAsync();

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns("wrongUsername");

        var saveSearchParams = fixture.Create<SaveSearchParams>();

        var command = new SaveSearchCommand { SaveSearchParams = saveSearchParams };
        var handler =
            new SaveSearchCommandHandler(context, httpContextAccessorMock.Object, userAccessorMock.Object, mapper);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeNullOrEmpty();
        result.IsEmpty.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
    }

    [Fact]
    public async Task Handler_ShouldReturnFailure_WhenSearchHasAlreadyAdded()
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
        var existingSavedSearch = user.SavedSearches.FirstOrDefault();
        existingSavedSearch.QueryString = "?" + existingSavedSearch.QueryString;

        context.AppUsers.Add(user);
        await context.SaveChangesAsync();

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var httpRequestMock = new Mock<HttpRequest>();
        var queryString = new QueryString($"{existingSavedSearch.QueryString}");
        httpRequestMock.SetupGet(r => r.QueryString).Returns(queryString);
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupGet(c => c.Request).Returns(httpRequestMock.Object);
        httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(httpContextMock.Object);

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(user.UserName);

        var saveSearchParams = fixture.Create<SaveSearchParams>();

        var command = new SaveSearchCommand { SaveSearchParams = saveSearchParams };
        var handler =
            new SaveSearchCommandHandler(context, httpContextAccessorMock.Object, userAccessorMock.Object, mapper);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("You have already added the same search.");
        result.IsEmpty.Should().BeFalse();
        result.Value.Should().Be(Unit.Value);
    }
}