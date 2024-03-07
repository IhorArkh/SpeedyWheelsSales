using System.Security.Claims;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Interfaces;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Ad.Commands.CreateAd;

public class CreateAdCommandHandler : IRequestHandler<CreateAdCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IUserAccessor _userAccessor;
    private readonly UserManager<AppUser> _userManager;

    public CreateAdCommandHandler(DataContext context, IMapper mapper, IUserAccessor userAccessor,
        UserManager<AppUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userAccessor = userAccessor;
        _userManager = userManager;
    }

    public async Task<Result<Unit>> Handle(CreateAdCommand request, CancellationToken cancellationToken)
    {
        var ad = _mapper.Map<Domain.Ad>(request.CreateAdDto);

        var currUserUsername = _userAccessor.GetUsername();
        if (currUserUsername is null)
            return Result<Unit>.Empty();

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == currUserUsername);
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