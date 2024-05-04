using AutoFixture;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Profile.Commands.UpdateUserProfile;
using SpeedyWheelsSales.Application.Features.Profile.Commands.UpdateUserProfile.DTOs;
using SpeedyWheelsSales.Application.Interfaces;

namespace SpeedyWheelsSales.Tests.Features.Profile.Commands;

public class UpdateUserProfileHandlerTests
{
    private const string ContextName = "DbForUpdateUserProfileCommandHandler";

    [Fact]
    public async Task Handler_ShouldReturnResultWithValidationError_WhenValidationDoesNotPassed()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var validatorMock = new Mock<IValidator<UpdateUserProfileDto>>();
        validatorMock.Setup(x =>
                x.ValidateAsync(It.IsAny<UpdateUserProfileDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult { Errors = fixture.CreateMany<ValidationFailure>(3).ToList() });

        var currUserAccessorMock = new Mock<ICurrentUserAccessor>();

        var userProfileDto = new UpdateUserProfileDto();

        var command = new UpdateUserProfileCommand { UpdateUserProfileDto = userProfileDto };
        var handler =
            new UpdateUserProfileCommandHandler(context, mapper, currUserAccessorMock.Object, validatorMock.Object);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.IsEmpty.Should().BeFalse();
        result.ValidationErrors.Count.Should().Be(3);
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Handler_ShouldReturnEmptyResult_WhenCouldNotGetUsernameFromCookie()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var validatorMock = new Mock<IValidator<UpdateUserProfileDto>>();
        validatorMock.Setup(x =>
                x.ValidateAsync(It.IsAny<UpdateUserProfileDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult { Errors = new List<ValidationFailure>() });

        var currUserAccessorMock = new Mock<ICurrentUserAccessor>();
        currUserAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(() => null);

        var userProfileDto = new UpdateUserProfileDto();

        var command = new UpdateUserProfileCommand { UpdateUserProfileDto = userProfileDto };
        var handler =
            new UpdateUserProfileCommandHandler(context, mapper, currUserAccessorMock.Object, validatorMock.Object);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.IsEmpty.Should().BeTrue();
        result.ValidationErrors.Count.Should().Be(0);
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeNullOrEmpty();
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

        var validatorMock = new Mock<IValidator<UpdateUserProfileDto>>();
        validatorMock.Setup(x =>
                x.ValidateAsync(It.IsAny<UpdateUserProfileDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult { Errors = new List<ValidationFailure>() });

        var currUserAccessorMock = new Mock<ICurrentUserAccessor>();
        currUserAccessorMock.Setup(x => x.GetCurrentUsername()).Returns("wrongUsername");

        var userProfileDto = fixture.Create<UpdateUserProfileDto>();

        var command = new UpdateUserProfileCommand { UpdateUserProfileDto = userProfileDto };
        var handler =
            new UpdateUserProfileCommandHandler(context, mapper, currUserAccessorMock.Object, validatorMock.Object);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.IsEmpty.Should().BeTrue();
        result.ValidationErrors.Count.Should().Be(0);
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Handler_ShouldReturnSuccessfulResult_WhenUserProfileUpdatedSuccessfully()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var validatorMock = new Mock<IValidator<UpdateUserProfileDto>>();
        validatorMock.Setup(x =>
                x.ValidateAsync(It.IsAny<UpdateUserProfileDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult { Errors = new List<ValidationFailure>() });

        var user = fixture.Create<AppUser>();
        context.AppUsers.Add(user);
        await context.SaveChangesAsync();

        var currUserAccessorMock = new Mock<ICurrentUserAccessor>();
        currUserAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(user.UserName);

        var userProfileDto = fixture.Create<UpdateUserProfileDto>();

        var command = new UpdateUserProfileCommand { UpdateUserProfileDto = userProfileDto };
        var handler =
            new UpdateUserProfileCommandHandler(context, mapper, currUserAccessorMock.Object, validatorMock.Object);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        var updatedUser = await context.AppUsers.SingleOrDefaultAsync();

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.IsEmpty.Should().BeFalse();
        result.ValidationErrors.Count.Should().Be(0);
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeNullOrEmpty();
        updatedUser.Should()
            .BeEquivalentTo(userProfileDto, x => x.ExcludingMissingMembers());
    }
}