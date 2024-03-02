using MediatR;

namespace SpeedyWheelsSales.Application.Ad.Commands.CreateAd;

public class CreateAdCommand : IRequest
{
    public Domain.Ad Ad { get; set; }
}