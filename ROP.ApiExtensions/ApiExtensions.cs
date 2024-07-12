using Microsoft.AspNetCore.Mvc;
using Murunu.ROP.Core;
using Murunu.ROP.ResultExtensions;

namespace Murunu.ROP.ApiExtensions;

public static partial class ApiExtensions
{
    public static async Task<IActionResult> AsOkObjectResult<T>(this Task<Result<T>> task)
    {
        var result = await task;
        return result.Match<T, IActionResult>(
            value => new OkObjectResult(value),
            error => new BadRequestObjectResult(error.Message));
    }

    public static async Task<IActionResult> AsCreatedResult<T>(this Task<Result<T>> task, string location)
    {
        var result = await task;
        return result.Match<T, IActionResult>(
            value => new CreatedResult(location, value),
            error => new BadRequestObjectResult(error.Message));
    }

    public static async Task<IActionResult> AsNoContentResult<T>(this Task<Result<T>> task)
    {
        var result = await task;
        return result.Match<T, IActionResult>(
            _ => new NoContentResult(),
            error => new BadRequestObjectResult(error.Message));
    }

    public static async Task<IActionResult> AsOkResult<T>(this Task<Result<T>> task)
    {
        var result = await task;
        return result.Match<T, IActionResult>(
            _ => new OkResult(),
            error => new BadRequestObjectResult(error.Message));
    }

    public static async Task<IActionResult> AsNotFoundResult<T>(this Task<Result<T>> task)
    {
        var result = await task;
        return result.Match<T, IActionResult>(
            _ => new NotFoundResult(),
            error => new BadRequestObjectResult(error.Message));
    }
}