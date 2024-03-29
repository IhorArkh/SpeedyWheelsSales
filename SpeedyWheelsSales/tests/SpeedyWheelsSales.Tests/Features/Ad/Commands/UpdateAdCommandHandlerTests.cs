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
using SpeedyWheelsSales.Application.Features.Ad.Commands.UpdateAd;
using SpeedyWheelsSales.Application.Features.Ad.Commands.UpdateAd.DTOs;

namespace SpeedyWheelsSales.Tests.Features.Ad.Commands;

public class UpdateAdCommandHandlerTests
{
    private const string ContextName = "DbForUpdateAdCommandHandler";

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

        var validatorMock = new Mock<IValidator<UpdateAdDto>>();
        validatorMock.Setup(x => x.ValidateAsync(It.IsAny<UpdateAdDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult { Errors = fixture.CreateMany<ValidationFailure>(3).ToList() });

        var currUserAccessorMock = new Mock<ICurrentUserAccessor>();

        var updateAdDto = new UpdateAdDto();

        var command = new UpdateAdCommand { UpdateAdDto = updateAdDto, Id = 222 };
        var handler = new UpdateAdCommandHandler(context, mapper, currUserAccessorMock.Object, validatorMock.Object);

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
    public async Task Handler_ShouldReturnEmptyResult_WhenValidationPassedAndAdDoesNotExist()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var validatorMock = new Mock<IValidator<UpdateAdDto>>();
        validatorMock.Setup(x => x.ValidateAsync(It.IsAny<UpdateAdDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult { Errors = new List<ValidationFailure>() });

        var currUserAccessorMock = new Mock<ICurrentUserAccessor>();

        var updateAdDto = new UpdateAdDto();

        var command = new UpdateAdCommand { UpdateAdDto = updateAdDto, Id = 222 };
        var handler = new UpdateAdCommandHandler(context, mapper, currUserAccessorMock.Object, validatorMock.Object);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.IsEmpty.Should().BeTrue();
        result.ValidationErrors.Should().BeNullOrEmpty();
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Handler_ShouldReturnFailure_WhenUsernameFromAdNotEqualsCurrentUsername()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var validatorMock = new Mock<IValidator<UpdateAdDto>>();
        validatorMock.Setup(x => x.ValidateAsync(It.IsAny<UpdateAdDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult { Errors = new List<ValidationFailure>() });

        var currUserAccessorMock = new Mock<ICurrentUserAccessor>();
        currUserAccessorMock.Setup(x => x.GetCurrentUsername()).Returns("otherUsername");

        var userManagerMock = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);

        var appUser = fixture.Create<AppUser>();
        appUser.Ads = new List<Domain.Entities.Ad>();
        appUser.FavouriteAds = new List<FavouriteAd>();

        var ad = fixture.Create<Domain.Entities.Ad>();
        ad.AppUser = appUser;

        context.Ads.Add(ad);
        await context.SaveChangesAsync();

        var updateAdDto = fixture.Create<UpdateAdDto>();

        var command = new UpdateAdCommand { UpdateAdDto = updateAdDto, Id = ad.Id };
        var handler = new UpdateAdCommandHandler(context, mapper, currUserAccessorMock.Object, validatorMock.Object);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.IsEmpty.Should().BeFalse();
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeEquivalentTo("Users can update only their own ads.");
        result.ValidationErrors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Handler_ShouldReturnSuccess_WhenAdUpdatedSuccessfully()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = new Fixture();

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var validatorMock = new Mock<IValidator<UpdateAdDto>>();
        validatorMock.Setup(x => x.ValidateAsync(It.IsAny<UpdateAdDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult { Errors = new List<ValidationFailure>() });

        var appUser = fixture.Create<AppUser>();
        appUser.Ads = new List<Domain.Entities.Ad>();
        appUser.FavouriteAds = new List<FavouriteAd>();

        var ad = fixture.Create<Domain.Entities.Ad>();
        ad.AppUser = appUser;

        context.Ads.Add(ad);
        await context.SaveChangesAsync();

        var currUserAccessorMock = new Mock<ICurrentUserAccessor>();
        currUserAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(appUser.UserName);

        var userManagerMock = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);

        var updateAdDto = fixture.Create<UpdateAdDto>();

        var command = new UpdateAdCommand { UpdateAdDto = updateAdDto, Id = ad.Id };
        var handler = new UpdateAdCommandHandler(context, mapper, currUserAccessorMock.Object, validatorMock.Object);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        var adFromDb = await context.Ads.SingleOrDefaultAsync();

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.IsEmpty.Should().BeFalse();
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeNullOrEmpty();
        result.ValidationErrors.Should().BeNullOrEmpty();

        adFromDb.Should().BeEquivalentTo(ad, opt => opt.ExcludingMissingMembers());
    }
}