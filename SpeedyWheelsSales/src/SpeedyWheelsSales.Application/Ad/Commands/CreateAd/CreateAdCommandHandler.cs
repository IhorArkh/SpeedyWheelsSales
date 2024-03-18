using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Ad.Commands.CreateAd.DTOs;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Commands.CreateAd;

public class CreateAdCommandHandler : IRequestHandler<CreateAdCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly UserManager<AppUser> _userManager;
    private readonly IValidator<CreateAdDto> _validator;

    public CreateAdCommandHandler(DataContext context, IMapper mapper, ICurrentUserAccessor currentUserAccessor,
        UserManager<AppUser> userManager, IValidator<CreateAdDto> validator)
    {
        _context = context;
        _mapper = mapper;
        _currentUserAccessor = currentUserAccessor;
        _userManager = userManager;
        _validator = validator;
    }

    public async Task<Result<Unit>> Handle(CreateAdCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request.CreateAdDto);
        if (!validationResult.IsValid)
            return Result<Unit>.ValidationError(validationResult.Errors);

        var ad = _mapper.Map<Domain.Entities.Ad>(request.CreateAdDto);

        var currUserUsername = _currentUserAccessor.GetCurrentUsername();
        if (currUserUsername is null)
            return Result<Unit>.Empty();

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == currUserUsername);
        // var user = await _userManager.FindByNameAsync(currUserUsername);
        if (user is null)
            return Result<Unit>.Empty();

        ad.AppUser = user;

        _context.Add(ad);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
            return Result<Unit>.Failure("Failed to create ad.");

        return Result<Unit>.Success(Unit.Value);
    }
}