using Murunu.ROP.Core;

namespace Murunu.ROP.ResultExtensions;

public static partial class ResultExtensions
{
    #region Return

    public static TOut Match<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> success, Func<Exception, TOut> failure) =>
        result.IsSuccess ? success(result.Value!) : failure(result.Exception!);

    public static async Task<TOut> Match<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, TOut> success,
        Func<Exception, TOut> failure)
    {
        var res = await result;
        
        return res.IsSuccess ? success(res.Value!) : failure(res.Exception!);
    }
    
    public static async Task<TOut> Match<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, Task<TOut>> success,
        Func<Exception, Task<TOut>> failure)
    {
        var res = await result;
        
        return res.IsSuccess ? await success(res.Value!) : await failure(res.Exception!);
    }
    
    public static async Task<TOut> Match<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, Task<TOut>> success,
        Func<Exception, TOut> failure)
    {
        var res = await result;
        
        return res.IsSuccess ? await success(res.Value!) : failure(res.Exception!);
    }
    
    public static async Task<TOut> Match<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, TOut> success,
        Func<Exception, Task<TOut>> failure)
    {
        var res = await result;
        
        return res.IsSuccess ? success(res.Value!) : await failure(res.Exception!);
    }

    #endregion

    #region Void

    public static void Match<TIn>(this Result<TIn> result, Action<TIn> success, Action<Exception> failure)
    {
        if (result.IsSuccess)
        {
            success(result.Value!);
        }
        else
        {
            failure(result.Exception!);
        }
    }
    
    public static async Task Match<TIn>(this Task<Result<TIn>> result, Action<TIn> success,
        Action<Exception> failure)
    {
        var res = await result;
        
        if (res.IsSuccess)
        {
            success(res.Value!);
        }
        else
        {
            failure(res.Exception!);
        }
    }
    
    public static async Task Match<TIn>(this Task<Result<TIn>> result, Func<TIn, Task> success,
        Func<Exception, Task> failure)
    {
        var res = await result;
        
        if (res.IsSuccess)
        {
            await success(res.Value!);
        }
        else
        {
            await failure(res.Exception!);
        }
    }
    
    public static async Task Match<TIn>(this Task<Result<TIn>> result, Func<TIn, Task> success,
        Action<Exception> failure)
    {
        var res = await result;
        
        if (res.IsSuccess)
        {
            await success(res.Value!);
        }
        else
        {
            failure(res.Exception!);
        }
    }
    
    public static async Task Match<TIn>(this Task<Result<TIn>> result, Action<TIn> success,
        Func<Exception, Task> failure)
    {
        var res = await result;
        
        if (res.IsSuccess)
        {
            success(res.Value!);
        }
        else
        {
            await failure(res.Exception!);
        }
    }

    #endregion
}