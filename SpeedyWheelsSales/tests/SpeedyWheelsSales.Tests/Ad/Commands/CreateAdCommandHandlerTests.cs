using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd;
using SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd.DTOs;

namespace SpeedyWheelsSales.Tests.Ad.Commands;

public class CreateAdCommandHandlerTests
{
    private const string ContextName = "DbForCreateAdCommandHandler";

    [Fact]
    public async Task Handler_ShouldCreateAd_WhenUserExistsAndValidationPassed()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var validatorMock = new Mock<IValidator<CreateAdDto>>();
        validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateAdDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult { Errors = new List<ValidationFailure>() });

        var user = fixture.Create<AppUser>();
        user.FavouriteAds = new List<FavouriteAd>();
        user.Ads = new List<Domain.Entities.Ad>();

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(user.UserName);

        var userManagerMock = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);

        userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);

        var createAdDto = fixture.Create<CreateAdDto>();

        var command = new CreateAdCommand { CreateAdDto = createAdDto };
        var handler = new CreateAdCommandHandler
            (context, mapper, userAccessorMock.Object, userManagerMock.Object, validatorMock.Object);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        var ad = await context.Ads.SingleOrDefaultAsync();

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeNullOrEmpty();
        result.ValidationErrors.Should().BeNullOrEmpty();
        ad.Should().BeEquivalentTo(createAdDto, opt => opt.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Handler_ShouldReturnResultWithValidationErrors_WhenValidationIsNotPassed()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var validatorMock = new Mock<IValidator<CreateAdDto>>();
        validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateAdDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult { Errors = fixture.CreateMany<ValidationFailure>(3).ToList() });

        var userAccessorMock = new Mock<ICurrentUserAccessor>();

        var userManagerMock = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);

        var createAdDto = fixture.Create<CreateAdDto>();

        var command = new CreateAdCommand { CreateAdDto = createAdDto };
        var handler = new CreateAdCommandHandler
            (context, mapper, userAccessorMock.Object, userManagerMock.Object, validatorMock.Object);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Count.Should().Be(3);
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Handler_ShouldReturnEmptyResult_WhenUserDoesNotExists()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var validatorMock = new Mock<IValidator<CreateAdDto>>();
        validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateAdDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult { Errors = new List<ValidationFailure>() });

        var user = fixture.Create<AppUser>();

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns("notExistingUserName");

        var userManagerMock = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);

        userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        var createAdDto = fixture.Create<CreateAdDto>();

        var command = new CreateAdCommand { CreateAdDto = createAdDto };
        var handler = new CreateAdCommandHandler
            (context, mapper, userAccessorMock.Object, userManagerMock.Object, validatorMock.Object);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.IsEmpty.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeNullOrEmpty();
        result.ValidationErrors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Handler_ShouldReturnEmptyResult_WhenCurrentUserAccessorReturnsNull()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var validatorMock = new Mock<IValidator<CreateAdDto>>();
        validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateAdDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult { Errors = new List<ValidationFailure>() });

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(() => null);

        var userManagerMock = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);

        var createAdDto = fixture.Create<CreateAdDto>();

        var command = new CreateAdCommand { CreateAdDto = createAdDto };
        var handler = new CreateAdCommandHandler
            (context, mapper, userAccessorMock.Object, userManagerMock.Object, validatorMock.Object);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.IsEmpty.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeNullOrEmpty();
        result.ValidationErrors.Should().BeNullOrEmpty();
    }
}