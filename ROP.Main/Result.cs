using ROP.Main.Helpers;

namespace ROP.Main;

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

    private Result(Exception exception)
    {
        Value = default;
        Exception = exception;
    }

    //happy path
    public static implicit operator Result<TValue>(TValue value) => new(value);

    //error path
    public static implicit operator Result<TValue> (Exception exception) => new(exception);
}

public class Result : Result<Unit>
{
    private Result() : base(Unit.Value)
    {
    }
    
    public static Result Success() => new();
}