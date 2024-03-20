using FluentValidation.Results;

namespace SpeedyWheelsSales.Application.Interfaces;

public interface IResult
{
    bool IsSuccess { get; }
    string Error { get; }
    bool IsEmpty { get; }
    List<ValidationFailure> ValidationErrors { get; }
}