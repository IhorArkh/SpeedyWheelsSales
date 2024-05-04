using MediatR;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Features.SavedSearch.Queries.GetSavedSearches;

public record GetSavedSearchesQuery : IRequest<Result<List<SavedSearchDto>>>
{
}