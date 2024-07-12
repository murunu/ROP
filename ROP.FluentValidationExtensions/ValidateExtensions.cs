using FluentValidation;
using Murunu.ROP.Core;

namespace Murunu.ROP.FluentValidationExtensions;

public static class ValidateExtensions
{
    public static async Task<Result<T>> Validate<T>(this Task<Result<T>> task, IValidator<T> validator)
    {
        var result = await task;

        if (result.IsFailure)
            return result;

        try
        {
            await validator.ValidateAndThrowAsync(result.Value!);

            return result;
        }
        catch (ValidationException e)
        {
            return e;
        }
    }
}