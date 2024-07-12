using Murunu.ROP.Core;
using Murunu.ROP.Helpers;

namespace Murunu.ROP.ResultExtensions;

public static partial class ResultExtensions
{
    public static async Task<Result<TOut>> Map<TIn, TOut>(this Task<Result<TIn>> task, Func<TIn, TOut> map)
    {
        var result = await task;
        return result.Match<TIn, Result<TOut>>(
            value => map(value),
            error => error);
    }
    
    public static async Task<Result<ResultDto<TOut>>> MapToDto<TIn, TOut>(this Task<Result<TIn>> task, Func<TIn, TOut> map)
    {
        var result = await task;

        return result.Match<TIn, ResultDto<TOut>>(
            value => new ResultDto<TOut> { Value = map(value) },
            error => new ResultDto<TOut> { Error = error.Message });
    }
}