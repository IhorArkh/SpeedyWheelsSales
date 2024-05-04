using MediatR;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Features.SavedSearch.Commands.SaveSearch;

public record SaveSearchCommand : IRequest<Result<Unit>>
{
    public SaveSearchParams SaveSearchParams { get; set; }
}