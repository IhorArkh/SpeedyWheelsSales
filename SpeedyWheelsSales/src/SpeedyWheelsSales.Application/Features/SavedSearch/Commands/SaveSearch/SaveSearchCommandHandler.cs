using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Application.Interfaces;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.SavedSearch.Commands.SaveSearch;

public class SaveSearchCommandHandler : IRequestHandler<SaveSearchCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IMapper _mapper;

    public SaveSearchCommandHandler(DataContext context, IHttpContextAccessor httpContextAccessor,
        ICurrentUserAccessor currentUserAccessor, IMapper mapper)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _currentUserAccessor = currentUserAccessor;
        _mapper = mapper;
    }

    public async Task<Result<Unit>> Handle(SaveSearchCommand request, CancellationToken cancellationToken)
    {
        var queryString = _httpContextAccessor.HttpContext.Request.QueryString.Value;
        if (string.IsNullOrEmpty(queryString))
            return Result<Unit>.Failure("Can't save search without parameters.");

        var currUserUsername = _currentUserAccessor.GetCurrentUsername();

        var user = await _context.AppUsers
            .Include(x => x.SavedSearches)
            .FirstOrDefaultAsync(x => x.UserName == currUserUsername);
        if (user is null)
            return Result<Unit>.Empty();

        if (user.SavedSearches.Any(x => x.QueryString == queryString))
            return Result<Unit>.Failure("You have already added the same search.");

        var savedSearch = _mapper.Map<Domain.Entities.SavedSearch>(request.SaveSearchParams);
        savedSearch.QueryString = queryString;

        user.SavedSearches.Add(savedSearch);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
            return Result<Unit>.Failure("Failed to save search.");

        return Result<Unit>.Success(Unit.Value);
    }
}