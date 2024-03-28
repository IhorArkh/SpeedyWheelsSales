using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.SavedSearch.Commands.DeleteSearch;

public class DeleteSearchCommandHandler : IRequestHandler<DeleteSearchCommand, Result<Unit>>
{
    private readonly DataContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public DeleteSearchCommandHandler(DataContext context, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<Unit>> Handle(DeleteSearchCommand request, CancellationToken cancellationToken)
    {
        var currUserUsername = _currentUserAccessor.GetCurrentUsername();

        var user = await _context.AppUsers
            .Include(x => x.SavedSearches)
            .FirstOrDefaultAsync(x => x.UserName == currUserUsername);
        if (user is null)
            return Result<Unit>.Empty();

        var savedSearch = user.SavedSearches.FirstOrDefault(x => x.Id == request.Id);
        if (savedSearch is null)
            return Result<Unit>.Failure("Search does not exist.");

        user.SavedSearches.Remove(savedSearch);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
            return Result<Unit>.Failure("Failed to save search.");

        return Result<Unit>.Success(Unit.Value);
    }
}