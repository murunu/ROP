using Murunu.ROP.Core;

namespace Murunu.ROP.ResultExtensions;

public static partial class ResultExtensions
{
    public static Result<(TIn, TOut)> Combine<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> action)
    {
        if (!result.IsSuccess)
        {
            return result.Exception!;
        }
        
        var res = action(result.Value!);
        return res.IsSuccess
            ? (result.Value!, res.Value!)
            : res.Exception!;
    }
    
    public static async Task<Result<(TIn, TOut)>> Combine<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<Result<TOut>>> action)
    {
        if (!result.IsSuccess)
        {
            return result.Exception!;
        }
        
        var res = await action(result.Value!);
        return res.IsSuccess
            ? (result.Value!, res.Value!)
            : res.Exception!;
    }

    public static async Task<Result<(TIn, TOut)>> Combine<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, Task<Result<TOut>>> action)
    {
        var res = await result;
        if (!res.IsSuccess)
        {
            return res.Exception!;
        }
        
        var actionRes = await action(res.Value!);
        return actionRes.IsSuccess
            ? (res.Value!, actionRes.Value!)
            : actionRes.Exception!;
    }

    public static async Task<Result<(TIn, TOut)>> Combine<TIn, TOut>(this Task<Result<TIn>> result, Func<TIn, Result<TOut>> action)
    {
        var res = await result;
        if (!res.IsSuccess)
        {
            return res.Exception!;
        }
        
        var actionRes = action(res.Value!);
        return actionRes.IsSuccess
            ? (res.Value!, actionRes.Value!)
            : actionRes.Exception!;
    }
}