using Murunu.ROP.Core;

namespace Murunu.ROP.ResultExtensions;

public static partial class ResultExtensions
{
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> func) =>
        result.Match(
            success: func,
            failure: exception => exception);

    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Result<TIn> result,
        Func<TIn, Task<Result<TOut>>> func) =>
        result.IsSuccess
            ? await func(result.Value!)
            : result.Exception!;

    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Task<Result<TIn>> resultTask,
        Func<TIn, Task<Result<TOut>>> func)
    {
        var result = await resultTask;
        
        return result.IsSuccess
            ? await func(result.Value!)
            : result.Exception!;
    }
    
    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Task<Result<TIn>> resultTask,
        Func<TIn, Result<TOut>> func)
    {
        var result = await resultTask;
        
        return result.IsSuccess
            ? func(result.Value!)
            : result.Exception!;
    }

    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> func)
    {
        try
        {
            return result.IsSuccess
                ? func(result.Value!)
                : result.Exception!;
        }
        catch (Exception e)
        {
            return e;
        }
    }
    
    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> func)
    {
        try
        {
            return result.IsSuccess
                ? await func(result.Value!)
                : result.Exception!;
        }
        catch (Exception e)
        {
            return e;
        }
    }
    
    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, Task<TOut>> func)
    {
        try
        {
            var taskResult = await result;
            return taskResult.IsSuccess
                ? await func(taskResult.Value!)
                : taskResult.Exception!;
        }
        catch (Exception e)
        {
            return e;
        }
    }
    
    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, TOut> func)
    {
        try
        {
            var taskResult = await result;
            return taskResult.IsSuccess
                ? func(taskResult.Value!)
                : taskResult.Exception!;
        }
        catch (Exception e)
        {
            return e;
        }
    }
}