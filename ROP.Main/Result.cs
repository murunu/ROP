namespace Murunu.ROP;

public class Result<TValue>
{
    internal Exception? Exception { get; }

    internal readonly TValue? Value;
    public bool IsSuccess => Value is not null;
    public bool IsFailure => !IsSuccess;

    internal Result(TValue value)
    {
        Value = value;
    }

    internal Result(Exception exception)
    {
        Value = default;
        Exception = exception;
    }

    //happy path
    public static implicit operator Result<TValue>(TValue value) => new(value);

    //error path
    public static implicit operator Result<TValue> (Exception exception) => new(exception);
    
    public static Result<T> Success<T>(T value) => new(value);
    public static Result<T> Failure<T>(Exception exception) => new(exception);
}

public class Result : Result<Unit>
{
    private Result() : base(Unit.Value)
    {
    }
    
    private Result(Exception exception) : base(exception)
    {
    }
    
    public static Result Success() => new();
    public static Result Failure(Exception e) => new(e);
}