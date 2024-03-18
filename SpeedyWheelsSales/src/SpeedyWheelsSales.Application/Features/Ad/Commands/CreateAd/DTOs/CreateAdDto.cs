namespace SpeedyWheelsSales.Application.Features.Ad.Commands.CreateAd.DTOs;

public class CreateAdDto
{
    public string Description { get; set; }
    public string City { get; set; }
    public CreateAdCarDto CreateAdCarDto { get; set; }
    public ICollection<CreateAdPhotoDto> CreateAdPhotoDtos { get; set; } = new List<CreateAdPhotoDto>();
}