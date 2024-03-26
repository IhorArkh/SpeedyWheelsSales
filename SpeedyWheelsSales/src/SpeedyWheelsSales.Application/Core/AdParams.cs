using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SpeedyWheelsSales.Application.Core;

public class AdParams : PagingParams
{
    public string? Model { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}