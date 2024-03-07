using FluentValidation.Results;

namespace SpeedyWheelsSales.Application.Core;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T Value { get; set; }
    public string Error { get; set; }
    public bool IsEmpty { get; set; }
    public List<ValidationFailure> ValidationErrors { get; set; } = new();

    public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };

    public static Result<T> Failure(string error) => new Result<T> { IsSuccess = false, Error = error };

    public static Result<T> Empty() => new Result<T> { IsEmpty = true };

    public static Result<T> ValidationError(List<ValidationFailure> errors) =>
        new Result<T> { ValidationErrors = errors };
}