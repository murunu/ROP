using Murunu.ROP.Core;
using Murunu.ROP.ResultExtensions;
using ROP.Tests.Helpers;

namespace ROP.Tests;

public class AsyncTests
{
    [Fact]
    public Task ShouldReturnErrorWhenErrorState()
        => RunsAsync()
            .Bind(_ => AsyncTaskError())
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Async Result Error", value.Exception!.Message));

    [Fact]
    public Task ShouldReturnValueWhenSuccess()
        => RunsAsync()
            .Bind(_ => ReturnsIntAsync())
            .Assert(value => Assert.Equal(1, value.Value));

    [Fact]
    public Task ShouldReturnExceptionWhenFailure()
        => RunsAsync()
            .Bind(_ => ReturnsIntThrowsAsync())
            .Assert(value => Assert.Equal("Async int Exception thrown", value.Exception!.Message));

    [Fact]
    public Task ShouldReturnSuccessWhenTapCalled()
        => RunsAsync()
            .Tap(_ => RunsAsync())
            .Assert(value => Assert.Equal("Async Result Success", value.Value));

    [Fact]
    public Task ShouldReturnSuccessWhenTapReturnsError()
        => RunsAsync()
            .Tap(_ => ReturnsErrorAsync())
            .Assert(value => Assert.Equal("Async Result Success", value.Value));

    [Fact]
    public Task ShouldReturnSuccessWhenRunSucceeds()
        => RunsAsync()
            .Assert(value => Assert.Equal("Async Result Success", value.Value));

    [Fact]
    public Task ShouldReturnErrorWhenExceptionIsThrown()
        => RunsAsync()
            .TryCatch(_ => ThrowAsync())
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Async Error thrown", value.Exception!.Message));

    [Fact]
    public Task ShouldThrowExceptionWhenErrorOccurs()
        => Assert.ThrowsAsync<Exception>(
            () => RunsAsync()
                .TryCatch(_ => ThrowAsync())
                .ThrowOnError());

    [Fact]
    public Task ShouldReturnSuccessWhenNoErrorOccurs()
        => RunsAsync()
            .ThrowOnError()
            .Assert(value => Assert.Equal("Async Result Success", value.Value));

    [Fact]
    public Task ShouldReturnErrorWhenErrorIsReturned()
        => ReturnsErrorAsync()
            .TryCatch(_ => RunsAsync())
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Async Error", value.Exception!.Message));

    [Fact]
    public Task ShouldReturnSuccessWhenSuccessIsReturned()
        => RunsAsync()
            .TryCatch(_ => RunsAsync())
            .Assert(value => Assert.Equal("Async Result Success", value.Value));

    [Fact]
    public Task ShouldReturnErrorWhenBindReturnsError()
        => ReturnsErrorAsync()
            .Bind(_ => RunsAsync())
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Async Error", value.Exception!.Message));

    [Fact]
    public Task ShouldHaveFailureWhenErrorReturned()
        => ReturnsErrorAsync()
            .Assert(value => Assert.True(value.IsFailure))
            .Assert(value => Assert.False(value.IsSuccess));

    [Fact]
    public Task ShouldHaveSuccessWhenSuccessReturned()
        => SuccessAsync()
            .Assert(value => Assert.True(value.IsSuccess))
            .Assert(value => Assert.False(value.IsFailure));

    [Fact]
    public Task ShouldHaveTupleWhenCombined()
        => RunsAsync()
            .Combine(_ => RunsAsync("2"))
            .Assert(value => Assert.IsType<Result<(string, string)>>(value))
            .Assert(value => Assert.Equal("Async Result Success", value.Value.Item1))
            .Assert(value => Assert.Equal("Async Result Success 2", value.Value.Item2));

    [Fact]
    public Task ShouldHaveExceptionWhenCombined()
        => ReturnsErrorAsync()
            .Combine(_ => RunsAsync("2"))
            .Assert(value => Assert.IsType<Exception>(value.Exception))
            .Assert(value => Assert.Null(value.Value.Item1))
            .Assert(value => Assert.Null(value.Value.Item2))
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Async Error", value.Exception!.Message));

    [Fact]
    public Task ShouldRunSuccessWhenMatch()
        => RunsAsync()
            .Match(success =>
                {
                    Assert.Equal("Async Result Success", success);

                    return success;
                },
                error =>
                {
                    Assert.True(false);

                    return error.Message;
                });

    [Fact]
    public Task ShouldRunFailureWhenMatch()
        => ReturnsErrorAsync()
            .Match(success =>
                {
                    Assert.True(false);

                    return success;
                },
                error =>
                {
                    Assert.Equal("Async Error", error.Message);

                    return error.Message;
                });
    
    [Fact]
    public Task ShouldRunSuccessWhenAsyncMatch()
        => RunsAsync()
            .Match(async success =>
                {
                    Assert.Equal("Async Result Success", success);

                    return await Task.FromResult(success);
                },
                error =>
                {
                    Assert.True(false);

                    return error.Message;
                });

    [Fact]
    public Task ShouldRunFailureWhenMatchAsyncFailure()
        => ReturnsErrorAsync()
            .Match(success =>
                {
                    Assert.True(false);

                    return success;
                },
                async error =>
                {
                    Assert.Equal("Async Error", error.Message);

                    return await Task.FromResult(error.Message);
                });
    
    [Fact]
    public Task ShouldRunSuccessWhenBothAsyncMatch()
        => RunsAsync()
            .Match(async success =>
                {
                    Assert.Equal("Async Result Success", success);

                    return await Task.FromResult(success);
                },
                async error =>
                {
                    Assert.True(false);

                    return await Task.FromResult(error.Message);
                });

    [Fact]
    public Task ShouldRunFailureWhenMatchBothAsyncFailure()
        => ReturnsErrorAsync()
            .Match(async success =>
                {
                    Assert.True(false);

                    return await Task.FromResult(success);
                },
                async error =>
                {
                    Assert.Equal("Async Error", error.Message);

                    return await Task.FromResult(error.Message);
                });

    [Fact]
    public Task ShouldReturnSuccessWhenContinueIfSuccess()
        => RunsAsync()
            .ContinueIf(_ => Task.FromResult(true))
            .Bind(_ => RunsAsync("2"))
            .Assert(value => Assert.Equal("Async Result Success 2", value.Value));
    
    [Fact]
    public Task ShouldReturnFailureWhenContinueIfNotSuccess()
        => RunsAsync()
            .ContinueIf(_ => Task.FromResult(false), new Exception("Not Success"))
            .Bind(_ => RunsAsync("2"))
            .Assert(value => Assert.Equal("Not Success", value.Exception!.Message));
    
    [Fact]
    public Task ShouldReturnFailureWhenInitialFailureNotSuccess()
        => ReturnsErrorAsync()
            .ContinueIf(_ => Task.FromResult(false), new Exception("Not Success"))
            .Bind(_ => RunsAsync("2"))
            .Assert(value => Assert.Equal("Async Error", value.Exception!.Message));
}