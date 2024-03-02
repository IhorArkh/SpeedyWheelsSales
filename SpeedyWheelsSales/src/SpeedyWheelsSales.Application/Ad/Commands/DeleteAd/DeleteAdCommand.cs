using MediatR;
using SpeedyWheelsSales.Application.Core;

namespace SpeedyWheelsSales.Application.Ad.Commands.DeleteAd;

public class DeleteAdCommand : IRequest<Result<Unit>>
{
    public int Id { get; set; }
}