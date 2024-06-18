namespace Murunu.ROP;

public static partial class ResultExtensions
{
    public static Result<TIn> ThrowOnError<TIn>(this Result<TIn> result)
    {
        if (!result.IsSuccess)
        {
            throw result.Exception;
        }

        return result;
    }
    
    public static async Task<Result<TIn>> ThrowOnError<TIn>(this Task<Result<TIn>> result)
    {
        var res = await result;
        if (!res.IsSuccess)
        {
            throw res.Exception;
        }

        return res;
    }
}