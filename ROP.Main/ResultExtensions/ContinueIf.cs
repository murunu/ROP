using Murunu.ROP.Core;

namespace Murunu.ROP.ResultExtensions;

public static partial class ResultExtensions
{
    public static Result<TIn> ContinueIf<TIn>(this Result<TIn> result, Func<TIn, bool> action,
        Exception? customException = null)
    {
        customException ??= new Exception("ContinueIf returned false");
        
        if (result.IsSuccess)
        {
            return action(result.Value!)
                ? result
                : customException;
        }

        return result;
    }
    
    public static async Task<Result<TIn>> ContinueIf<TIn>(this Result<TIn> result, Func<TIn, Task<bool>> action,
        Exception? customException = null)
    {
        customException ??= new Exception("ContinueIf returned false");
        
        if (result.IsSuccess)
        {
            return await action(result.Value!)
                ? result
                : customException;
        }

        return result;
    }
    
    public static async Task<Result<TIn>> ContinueIf<TIn>(this Task<Result<TIn>> result, Func<TIn, bool> action,
        Exception? customException = null)
    {
        customException ??= new Exception("ContinueIf returned false");

        var taskResult = await result;
        
        if (taskResult.IsSuccess)
        {
            return action(taskResult.Value!)
                ? taskResult
                : customException;
        }

        return taskResult;
    }
    
    public static async Task<Result<TIn>> ContinueIf<TIn>(this Task<Result<TIn>> result, Func<TIn, Task<bool>> action,
        Exception? customException = null)
    {
        customException ??= new Exception("ContinueIf returned false");

        var taskResult = await result;
        
        if (taskResult.IsSuccess)
        {
            return await action(taskResult.Value!)
                ? taskResult
                : customException;
        }

        return taskResult;
    }
    
    public static async Task<Result<TIn>> ContinueIf<TIn>(this Task<Result<TIn>> result, Func<TIn, Task<Result<bool>>> action,
        Exception? customException = null)
    {
        customException ??= new Exception("ContinueIf returned false");

        var taskResult = await result;

        if (!taskResult.IsSuccess) 
            return taskResult;
        
        var res = await action(taskResult.Value!);
        return res.IsSuccess
            ? taskResult
            : customException;

    }
}