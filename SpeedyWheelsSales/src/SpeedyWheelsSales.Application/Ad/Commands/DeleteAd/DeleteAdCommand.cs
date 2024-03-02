using MediatR;

namespace SpeedyWheelsSales.Application.Ad.Commands.DeleteAd;

public class DeleteAdCommand : IRequest
{
    public int Id { get; set; }
}