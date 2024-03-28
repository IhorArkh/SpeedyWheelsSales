using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpeedyWheelsSales.Application.Core;
using SpeedyWheelsSales.Infrastructure.Data;

namespace SpeedyWheelsSales.Application.Features.SavedSearch.Queries.GetSavedSearches;

public class GetSavedSearchesQueryHandler : IRequestHandler<GetSavedSearchesQuery, Result<List<SavedSearchDto>>>
{
    private readonly DataContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IMapper _mapper;

    public GetSavedSearchesQueryHandler(DataContext context, ICurrentUserAccessor currentUserAccessor, IMapper mapper)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
        _mapper = mapper;
    }

    public async Task<Result<List<SavedSearchDto>>> Handle(GetSavedSearchesQuery request,
        CancellationToken cancellationToken)
    {
        var currUserUsername = _currentUserAccessor.GetCurrentUsername();

        var user = await _context.AppUsers
            .Include(x => x.SavedSearches)
            .FirstOrDefaultAsync(x => x.UserName == currUserUsername);
        if (user is null)
            return Result<List<SavedSearchDto>>.Empty();

        var savedSearches = user.SavedSearches.ToList();

        var savedSearchesDto = _mapper.Map<List<SavedSearchDto>>(savedSearches);

        return Result<List<SavedSearchDto>>.Success(savedSearchesDto);
    }
}