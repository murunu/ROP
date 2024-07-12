using Murunu.ROP.Core;

namespace Murunu.ROP.ResultExtensions;

public static partial class ResultExtensions
{
    public static Result<TOut> TryCatch<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> func)
    {
        try
        {
            return result.Match(
                func,
                failure => failure);
        }
        catch (Exception e)
        {
            return e;
        }
    }
    
    public static async Task<Result<TOut>> TryCatch<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> func)
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
    
    public static async Task<Result<TOut>> TryCatch<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, Task<Result<TOut>>> func)
    {
        try
        {
            var res = await result;
            return res.IsSuccess
                ? await func(res.Value!)
                : res.Exception!;
        }
        catch (Exception e)
        {
            return e;
        }
    }
    
    public static async Task<Result<TOut>> TryCatch<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, Result<TOut>> func)
    {
        try
        {
            var res = await result;
            return res.IsSuccess
                ? func(res.Value!)
                : res.Exception!;
        }
        catch (Exception e)
        {
            return e;
        }
    }
}