namespace Murunu.ROP;

public static partial class ResultExtensions
{
    /// <summary>
    /// Convert a Result to a <see cref="Task"/>
    /// </summary>
    /// <returns>A <see cref="Task"/> of <see cref="Result{TValue}"/></returns>
    public static Task<Result<T>> Async<T>(this Result<T> result) => Task.FromResult(result);
}