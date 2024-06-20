namespace Murunu.ROP;

public static partial class ResultExtensions
{
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
}