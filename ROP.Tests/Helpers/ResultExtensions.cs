namespace ROP.Tests.Helpers;

public static class ResultExtensions
{
    public static Result<TIn> Assert<TIn>(this Result<TIn> result, Action<Result<TIn>> assert)
    {
        assert(result);

        return result;
    }
    
    public static async Task<Result<TIn>> Assert<TIn>(this Task<Result<TIn>> result, Action<Result<TIn>> assert)
    {
        var taskResult = await result;
        
        assert(taskResult);

        return taskResult;
    }
    
    public static async Task<Result> Assert(this Task<Result> result, Action<Result> assert)
    {
        var taskResult = await result;
        
        assert(taskResult);

        return taskResult;
    }
}