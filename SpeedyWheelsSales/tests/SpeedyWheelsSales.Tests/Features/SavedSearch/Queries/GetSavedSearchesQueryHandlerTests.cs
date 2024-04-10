using AutoFixture;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.SavedSearch.Queries.GetSavedSearches;
using SpeedyWheelsSales.Application.Interfaces;

namespace SpeedyWheelsSales.Tests.Features.SavedSearch.Queries;

public class GetSavedSearchesQueryHandlerTests
{
    private const string ContextName = "DbForGetSavedSearchesQueryHandler";

    [Fact]
    public async Task Handler_ShouldReturnListWithsavedSearches_WhenExists()
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

        var query = new GetSavedSearchesQuery();
        var handler = new GetSavedSearchesQueryHandler(context, userAccessorMock.Object, mapper);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.IsEmpty.Should().BeFalse();
        result.Error.Should().BeNullOrEmpty();
        result.Value.Should().BeEquivalentTo(user.SavedSearches, opt =>
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

        var query = new GetSavedSearchesQuery();
        var handler = new GetSavedSearchesQueryHandler(context, userAccessorMock.Object, mapper);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.IsEmpty.Should().BeTrue();
        result.Error.Should().BeNullOrEmpty();
        result.Value.Should().BeNullOrEmpty();
    }
}