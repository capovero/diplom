// Result.cs
namespace backendtest.Services;

public class Result<T>
{
    public T Value { get; }
    public Error Error { get; }
    public bool IsSuccess { get; }

    private Result(T value)
    {
        Value = value;
        IsSuccess = true;
    }

    private Result(Error error)
    {
        Error = error;
        IsSuccess = false;
    }

    public static Result<T> Success(T value) => new Result<T>(value);
    public static Result<T> Failure(Error error) => new Result<T>(error);

    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<Error, TResult> onFailure) 
        => IsSuccess ? onSuccess(Value) : onFailure(Error);
}

public record Error(string Message);