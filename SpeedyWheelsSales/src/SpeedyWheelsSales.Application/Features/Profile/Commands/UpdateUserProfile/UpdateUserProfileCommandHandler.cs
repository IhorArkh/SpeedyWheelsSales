using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Features.Profile.Commands.UpdateUserProfile.DTOs;
using SpeedyWheelsSales.Application.Interfaces;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.Profile.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IValidator<UpdateUserProfileDto> _validator;

    public UpdateUserProfileCommandHandler(
        DataContext context,
        IMapper mapper,
        ICurrentUserAccessor currentUserAccessor,
        IValidator<UpdateUserProfileDto> validator)
    {
        _context = context;
        _mapper = mapper;
        _currentUserAccessor = currentUserAccessor;
        _validator = validator;
    }

    public async Task<Result<Unit>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request.UpdateUserProfileDto);
        if (!validationResult.IsValid)
            return Result<Unit>.ValidationError(validationResult.Errors);

        var currUsername = _currentUserAccessor.GetCurrentUsername();
        if (currUsername is null)
            return Result<Unit>.Empty();

        var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserName == currUsername);
        if (user == default)
            return Result<Unit>.Empty();

        _mapper.Map(request.UpdateUserProfileDto, user);

        var result = await _context.SaveChangesAsync() > 0;
        if (!result)
            return Result<Unit>.Failure("Failed to update user profile.");

        return Result<Unit>.Success(Unit.Value);
    }
}