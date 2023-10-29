using HomeworkManager.Model.ErrorEntities;

namespace HomeworkManager.Model.CustomEntities;

public class Result<TValue, TError>
    where TError : BusinessError
{
    public TValue? Value { get; }
    public TError? Error { get; }
    public bool Success { get; }

    private Result(TValue? value, TError? error, bool success)
    {
        Value = value;
        Error = error;
        Success = success;
    }

    public static Result<TValue, TError> Ok(TValue value)
    {
        return new Result<TValue, TError>(value, null, true);
    }

    public static Result<TValue, TError> Err(TError error)
    {
        return new Result<TValue, TError>(default, error, false);
    }

    public static implicit operator Result<TValue, TError>(TValue value)
    {
        return new Result<TValue, TError>(value, null, true);
    }

    public static implicit operator Result<TValue, TError>(TError error)
    {
        return new Result<TValue, TError>(default, error, false);
    }

    public TResult Match<TResult>(
        Func<TValue, TResult> success,
        Func<TError, TResult> failure)
    {
        return Success ? success(Value!) : failure(Error!);
    }
}