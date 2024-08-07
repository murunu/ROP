﻿using Murunu.ROP.Core;

namespace Murunu.ROP.ResultExtensions;

public static partial class ResultExtensions
{
    public static Result<TIn> Tap<TIn>(this Result<TIn> result, Action<TIn> action)
    {
        if (result.IsSuccess)
        {
            action(result.Value!);
        }

        return result;
    }
    
    public static async Task<Result<TIn>> Tap<TIn>(this Task<Result<TIn>> result, Action<TIn> action)
    {
        var res = await result;
        if (res.IsSuccess)
        {
            action(res.Value!);
        }

        return res;
    }
    
    public static async Task<Result<TIn>> Tap<TIn>(this Task<Result<TIn>> result, Func<TIn, Task> action)
    {
        var res = await result;
        if (res.IsSuccess)
        {
            await action(res.Value!);
        }

        return res;
    }
}