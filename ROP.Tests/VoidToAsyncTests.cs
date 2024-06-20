using ROP.Tests.Helpers;

namespace ROP.Tests;

public class VoidToAsyncTests
{
    [Fact]
    public Task ShouldReturnErrorWhenErrorState()
        => Runs()
            .Bind(_ => AsyncTaskError())
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Async Result Error", value.Exception!.Message));

    [Fact]
    public Task ShouldReturnValueWhenSuccess()
        => Runs()
            .Bind(_ => ReturnsIntAsync())
            .Assert(value => Assert.Equal(1, value.Value));

    [Fact]
    public Task ShouldReturnExceptionWhenFailure()
        => Runs()
            .Bind(_ => ReturnsIntThrowsAsync())
            .Assert(value => Assert.Equal("Async int Exception thrown", value.Exception!.Message));

    [Fact]
    public Task ShouldReturnSuccessWhenTapCalled()
        => Runs()
            .Async()
            .Tap(_ => RunsAsync())
            .Assert(value => Assert.Equal("Success", value.Value));

    [Fact]
    public Task ShouldReturnSuccessWhenTapReturnsError()
        => Runs()
            .Async()
            .Tap(_ => ReturnsErrorAsync())
            .Assert(value => Assert.Equal("Success", value.Value));

    [Fact]
    public Task ShouldReturnErrorWhenExceptionIsThrown()
        => Runs()
            .TryCatch(_ => ThrowAsync())
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Async Error thrown", value.Exception!.Message));

    [Fact]
    public Task ShouldThrowExceptionWhenErrorOccurs()
        => Assert.ThrowsAsync<Exception>(
            () => Runs()
                .TryCatch(_ => ThrowAsync())
                .ThrowOnError());

    [Fact]
    public Task ShouldReturnErrorWhenErrorIsReturned()
        => ReturnsError()
            .TryCatch(_ => RunsAsync())
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Error", value.Exception!.Message));

    [Fact]
    public Task ShouldReturnErrorWhenBindReturnsError()
        => ReturnsError()
            .Bind(_ => RunsAsync())
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Error", value.Exception!.Message));

    [Fact]
    public Task ShouldHaveTupleWhenCombined()
        => Runs()
            .Combine(_ => RunsAsync("2"))
            .Assert(value => Assert.IsType<Result<(string, string)>>(value))
            .Assert(value => Assert.Equal("Success", value.Value.Item1))
            .Assert(value => Assert.Equal("Async Result Success 2", value.Value.Item2));

    [Fact]
    public Task ShouldHaveExceptionWhenCombined()
        => ReturnsError()
            .Combine(_ => RunsAsync("2"))
            .Assert(value => Assert.IsType<Exception>(value.Exception))
            .Assert(value => Assert.Null(value.Value.Item1))
            .Assert(value => Assert.Null(value.Value.Item2))
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Error", value.Exception!.Message));

    [Fact]
    public Task ShouldBeTaskWhenAsync()
        => Runs()
            .Async()
            .Assert(value => Assert.IsType<Result<string>>(value));
    
    [Fact]
    public Task ShouldReturnSuccessWhenContinueIfSuccess()
        => Runs()
            .ContinueIf(_ => Task.FromResult(true))
            .Bind(_ => Runs("2"))
            .Assert(value => Assert.Equal("Success 2", value.Value));
    
    [Fact]
    public Task ShouldReturnFailureWhenContinueIfNotSuccess()
        => Runs()
            .ContinueIf(_ => Task.FromResult(false), new Exception("Not Success"))
            .Bind(_ => Runs("2"))
            .Assert(value => Assert.Equal("Not Success", value.Exception!.Message));
    
    [Fact]
    public Task ShouldReturnFailureWhenInitialFailureNotSuccess()
        => ReturnsError()
            .ContinueIf(_ => Task.FromResult(false), new Exception("Not Success"))
            .Bind(_ => Runs("2"))
            .Assert(value => Assert.Equal("Error", value.Exception!.Message));
}