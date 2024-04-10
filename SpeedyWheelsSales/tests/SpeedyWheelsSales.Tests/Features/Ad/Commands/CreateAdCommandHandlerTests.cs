using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd;
using SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd.DTOs;
using SpeedyWheelsSales.Application.Interfaces;
using SpeedyWheelsSales.Application.Photos;

namespace SpeedyWheelsSales.Tests.Features.Ad.Commands;

public class CreateAdCommandHandlerTests
{
    private const string ContextName = "DbForCreateAdCommandHandler";

    [Fact]
    public async Task Handler_ShouldCreateAd_WhenUserExistsAndValidationPassed()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = CreateAndConfigureAutoFixture();

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var validatorMock = new Mock<IValidator<CreateAdDto>>();
        validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateAdDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult { Errors = new List<ValidationFailure>() });

        var user = fixture.Create<AppUser>();
        user.FavouriteAds = new List<FavouriteAd>();
        user.Ads = new List<Domain.Entities.Ad>();

        context.AppUsers.Add(user);
        await context.SaveChangesAsync();

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(user.UserName);

        var photoAccessorMock = new Mock<IPhotoAccessor>();
        var publicIdCounter = 0;
        photoAccessorMock.Setup(x => x.AddPhoto(It.IsAny<IFormFile>()))
            .ReturnsAsync(() =>
            {
                publicIdCounter++;
                var publicId = "PublicId_" + publicIdCounter;
                var url = fixture.Create<string>();
                return new PhotoUploadResult()
                {
                    PublicId = publicId,
                    Url = url
                };
            });

        var createAdDto = fixture.Create<CreateAdDto>();

        var command = new CreateAdCommand { CreateAdDto = createAdDto };
        var handler = new CreateAdCommandHandler(
            context,
            mapper,
            userAccessorMock.Object,
            validatorMock.Object,
            photoAccessorMock.Object);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        var ad = await context.Ads.SingleOrDefaultAsync();

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeNullOrEmpty();
        result.ValidationErrors.Should().BeNullOrEmpty();
        ad.Should().BeEquivalentTo(createAdDto, opt => opt.ExcludingMissingMembers());
        ad.CreatedAt.Date.Should().Be(DateTime.UtcNow.Date);
        ad.Photos.Count.Should().Be(3);
    }

    [Fact]
    public async Task Handler_ShouldReturnResultWithValidationErrors_WhenValidationIsNotPassed()
    {
        //Arrange
        var context = await InMemoryDbContextProvider.GetDbContext(ContextName);
        var fixture = CreateAndConfigureAutoFixture();

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var validatorMock = new Mock<IValidator<CreateAdDto>>();
        validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateAdDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult { Errors = fixture.CreateMany<ValidationFailure>(3).ToList() });

        var userAccessorMock = new Mock<ICurrentUserAccessor>();

        var photoAccessorMock = new Mock<IPhotoAccessor>();

        var createAdDto = fixture.Create<CreateAdDto>();

        var command = new CreateAdCommand { CreateAdDto = createAdDto };
        var handler = new CreateAdCommandHandler(
            context,
            mapper,
            userAccessorMock.Object,
            validatorMock.Object,
            photoAccessorMock.Object);

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
        var fixture = CreateAndConfigureAutoFixture();

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var validatorMock = new Mock<IValidator<CreateAdDto>>();
        validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateAdDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult { Errors = new List<ValidationFailure>() });

        var user = fixture.Create<AppUser>();

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns("notExistingUserName");

        var photoAccessorMock = new Mock<IPhotoAccessor>();

        var createAdDto = fixture.Create<CreateAdDto>();

        var command = new CreateAdCommand { CreateAdDto = createAdDto };
        var handler = new CreateAdCommandHandler(
            context,
            mapper,
            userAccessorMock.Object,
            validatorMock.Object,
            photoAccessorMock.Object);

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
        var fixture = CreateAndConfigureAutoFixture();

        var mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfiles>()));

        var validatorMock = new Mock<IValidator<CreateAdDto>>();
        validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateAdDto>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult { Errors = new List<ValidationFailure>() });

        var userAccessorMock = new Mock<ICurrentUserAccessor>();
        userAccessorMock.Setup(x => x.GetCurrentUsername()).Returns(() => null);

        var photoAccessorMock = new Mock<IPhotoAccessor>();

        var createAdDto = fixture.Create<CreateAdDto>();

        var command = new CreateAdCommand { CreateAdDto = createAdDto };
        var handler = new CreateAdCommandHandler(
            context,
            mapper,
            userAccessorMock.Object,
            validatorMock.Object,
            photoAccessorMock.Object);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.IsEmpty.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
        result.Error.Should().BeNullOrEmpty();
        result.ValidationErrors.Should().BeNullOrEmpty();
    }

    private IFixture CreateAndConfigureAutoFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        fixture.Register(() =>
        {
            var mock = new Mock<IFormFile>();
            mock.Setup(m => m.FileName).Returns(fixture.Create<string>());
            return mock.Object;
        });

        return fixture;
    }
}