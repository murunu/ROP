namespace ROP.Tests.Helpers;

public static class HelperMethods
{
    public static Result<string> Runs()
    {
        return "Success";
    }
    
    public static Result<string> Runs(string input)
    {
        return "Success " + input;
    }
    
    public static Task<Result<string>> RunsAsync()
    {
        return Task.FromResult<Result<string>>("Async Result Success");
    }
    
    public static Task<Result<string>> RunsAsync(string input)
    {
        return Task.FromResult<Result<string>>("Async Result Success " + input);
    }

    public static Result<string> Throw()
    {
        throw new Exception("Error thrown");
    }
    
    public static Task<Result<string>> ThrowAsync()
    {
        throw new Exception("Async Error thrown");
    }

    public static Result<string> ReturnsError()
    {
        return new Exception("Error");
    }
    
    public static Task<Result<string>> ReturnsErrorAsync()
    {
        return Task.FromResult<Result<string>>(new Exception("Async Error"));
    }
    
    public static Task<Result<string>> AsyncTaskError()
    {
        return Task.FromResult<Result<string>>(new Exception("Async Result Error"));
    }
    
    public static Result Success() => Result.Success();
    public static Task<Result> SuccessAsync() => Task.FromResult(Result.Success());
}