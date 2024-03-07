namespace SpeedyWheelsSales.Application.Core;

public class ValidationError
{
    public string PropertyName { get; set; }
    public string AttemptedValue { get; set; }
    public string ErrorMessage { get; set; }
}