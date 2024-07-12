using Murunu.ROP.Core;

namespace Murunu.ROP.ResultExtensions;

public static partial class ResultExtensions
{
    public static async Task<Result<T>> AsResult<T>(this Task<T> task)
    {
        try
        {
            return await task;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public static async Task<Result<T>> AsResult<T>(this ValueTask<T> task)
    {
        try
        {
            return await task;
        }
        catch (Exception e)
        {
            return e;
        }
    }
}