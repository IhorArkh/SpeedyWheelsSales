using MediatR;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Features.SavedSearch.Queries.GetSavedSearches;

public class GetSavedSearchesQuery : IRequest<Result<List<SavedSearchDto>>>
{
}