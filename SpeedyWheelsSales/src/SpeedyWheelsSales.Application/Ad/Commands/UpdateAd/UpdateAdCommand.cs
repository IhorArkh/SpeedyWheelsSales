using MediatR;

namespace SpeedyWheelsSales.Application.Ad.Commands.UpdateAd;

public class UpdateAdCommand : IRequest
{
    public Domain.Ad Ad { get; set; }
}