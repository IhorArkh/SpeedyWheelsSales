using MediatR;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Features.SavedSearch.Commands.DeleteSearch;

public class DeleteSearchCommand : IRequest<Result<Unit>>
{
    public int Id { get; set; }
}