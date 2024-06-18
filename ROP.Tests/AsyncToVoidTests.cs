using ROP.Tests.Helpers;

namespace ROP.Tests;

public class AsyncToVoidTests
{
    [Fact]
    public Task ShouldReturnErrorWhenErrorState()
        => RunsAsync()
            .Bind(_ => ReturnsError())
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Error", value.Exception!.Message));

    [Fact]
    public Task ShouldReturnValueWhenSuccess()
        => RunsAsync()
            .Bind(_ => ReturnsInt())
            .Assert(value => Assert.Equal(1, value.Value));

    [Fact]
    public Task ShouldReturnExceptionWhenFailure()
        => RunsAsync()
            .Bind(_ => ReturnsIntThrows())
            .Assert(value => Assert.Equal("Int Exception thrown", value.Exception.Message));

    [Fact]
    public Task ShouldReturnSuccessWhenTapCalled()
        => RunsAsync()
            .Tap(_ => Runs())
            .Assert(value => Assert.Equal("Async Result Success", value.Value));

    [Fact]
    public Task ShouldReturnSuccessWhenTapReturnsError()
        => RunsAsync()
            .Tap(_ => ReturnsError())
            .Assert(value => Assert.Equal("Async Result Success", value.Value));

    [Fact]
    public Task ShouldReturnErrorWhenExceptionIsThrown()
        => RunsAsync()
            .TryCatch(_ => Throw())
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Error thrown", value.Exception!.Message));

    [Fact]
    public Task ShouldThrowExceptionWhenErrorOccurs()
        => Assert.ThrowsAsync<Exception>(
            () => RunsAsync()
                .TryCatch(_ => Throw())
                .ThrowOnError());

    [Fact]
    public Task ShouldReturnErrorWhenErrorIsReturned()
        => ReturnsErrorAsync()
            .TryCatch(_ => Runs())
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Async Error", value.Exception!.Message));

    [Fact]
    public Task ShouldReturnErrorWhenBindReturnsError()
        => ReturnsErrorAsync()
            .Bind(_ => Runs())
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Async Error", value.Exception!.Message));

    [Fact]
    public Task ShouldHaveTupleWhenCombined()
        => RunsAsync()
            .Combine(_ => Runs("2"))
            .Assert(value => Assert.IsType<Result<(string, string)>>(value))
            .Assert(value => Assert.Equal("Async Result Success", value.Value.Item1))
            .Assert(value => Assert.Equal("Success 2", value.Value.Item2));

    [Fact]
    public Task ShouldHaveExceptionWhenCombined()
        => ReturnsErrorAsync()
            .Combine(_ => Runs("2"))
            .Assert(value => Assert.IsType<Exception>(value.Exception))
            .Assert(value => Assert.Null(value.Value.Item1))
            .Assert(value => Assert.Null(value.Value.Item2))
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Async Error", value.Exception!.Message));
}