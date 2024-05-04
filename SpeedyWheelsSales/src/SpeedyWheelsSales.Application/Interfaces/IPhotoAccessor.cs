using Microsoft.AspNetCore.Http;
using SpeedyWheelsSales.Application.Photos;

namespace SpeedyWheelsSales.Application.Interfaces;

public interface IPhotoAccessor
{
    Task<PhotoUploadResult> AddPhoto(IFormFile file);
    Task<string> DeletePhoto(string publicId);
}