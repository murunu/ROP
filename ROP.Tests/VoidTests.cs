using ROP.Tests.Helpers;

namespace ROP.Tests;

public class VoidTests
{
    [Fact]
    public void ShouldReturnErrorWhenErrorState()
        => Runs()
            .Bind(_ => ReturnsError())
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Error", value.Exception!.Message));

    [Fact]
    public void ShouldReturnSuccessWhenSuccess()
        => Runs()
            .Bind(_ => Runs())
            .Assert(value => Assert.Equal("Success", value.Value));

    [Fact]
    public void ShouldReturnExceptionWhenFailure()
        => Runs()
            .Bind(_ => ReturnsIntThrows())
            .Assert(value => Assert.Equal("Int Exception thrown", value.Exception!.Message));

    [Fact]
    public void ShouldReturnValueWhenSuccess()
        => Runs()
            .Bind(_ => ReturnsInt())
            .Assert(value => Assert.Equal(1, value.Value));

    [Fact]
    public void ShouldReturnSuccessWhenTapCalled()
        => Runs()
            .Tap(_ => Runs())
            .Assert(value => Assert.Equal("Success", value.Value));

    [Fact]
    public void ShouldReturnSuccessWhenTapReturnsError()
        => Runs()
            .Tap(_ => ReturnsError())
            .Assert(value => Assert.Equal("Success", value.Value));

    [Fact]
    public void ShouldReturnSuccessWhenRunSucceeds()
        => Runs()
            .Assert(value => Assert.Equal("Success", value.Value));

    [Fact]
    public void ShouldReturnErrorWhenExceptionIsThrown()
        => Runs()
            .TryCatch(_ => Throw())
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Error thrown", value.Exception!.Message));

    [Fact]
    public void ShouldThrowExceptionWhenErrorOccurs()
        => Assert.Throws<Exception>(
            () => Runs()
                .TryCatch(_ => Throw())
                .ThrowOnError());

    [Fact]
    public void ShouldReturnSuccessWhenNoErrorOccurs()
        => Runs()
            .ThrowOnError()
            .Assert(value => Assert.Equal("Success", value.Value));

    [Fact]
    public void ShouldReturnErrorWhenErrorIsReturned()
        => ReturnsError()
            .TryCatch(_ => Runs())
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Error", value.Exception!.Message));

    [Fact]
    public void ShouldReturnSuccessWhenSuccessIsReturned()
        => Runs()
            .TryCatch(_ => Runs())
            .Assert(value => Assert.Equal("Success", value.Value));

    [Fact]
    public void ShouldReturnErrorWhenBindReturnsError()
        => ReturnsError()
            .Bind(_ => Runs())
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Error", value.Exception!.Message));

    [Fact]
    public void ShouldHaveFailureWhenErrorReturned()
        => ReturnsError()
            .Assert(value => Assert.True(value.IsFailure))
            .Assert(value => Assert.False(value.IsSuccess));

    [Fact]
    public void ShouldHaveSuccessWhenSuccessReturned()
        => Success()
            .Assert(value => Assert.True(value.IsSuccess))
            .Assert(value => Assert.False(value.IsFailure));

    [Fact]
    public void ShouldHaveTupleWhenCombined()
        => Runs()
            .Combine(_ => Runs("2"))
            .Assert(value => Assert.IsType<Result<(string, string)>>(value))
            .Assert(value => Assert.Equal("Success", value.Value.Item1))
            .Assert(value => Assert.Equal("Success 2", value.Value.Item2));

    [Fact]
    public void ShouldHaveExceptionWhenCombined()
        => ReturnsError()
            .Combine(_ => Runs("2"))
            .Assert(value => Assert.IsType<Exception>(value.Exception))
            .Assert(value => Assert.Null(value.Value.Item1))
            .Assert(value => Assert.Null(value.Value.Item2))
            .Assert(value => Assert.NotNull(value.Exception))
            .Assert(value => Assert.Equal("Error", value.Exception!.Message));

    [Fact]
    public void ShouldRunSuccessWhenMatch()
        => Runs()
            .Match(success =>
                {
                    Assert.Equal("Success", success);

                    return success;
                },
                error =>
                {
                    Assert.True(false);

                    return error.Message;
                });

    [Fact]
    public void ShouldRunFailureWhenMatch()
        => ReturnsError()
            .Match(success =>
                {
                    Assert.True(false);

                    return success;
                },
                error =>
                {
                    Assert.Equal("Error", error.Message);

                    return error.Message;
                });

    [Fact]
    public void ShouldReturnFailureWhenUnitFailure()
        => Result.Failure(new Exception("Not success"))
            .Assert(value => Assert.Equal("Not success", value.Exception!.Message));

    [Fact]
    public void ShouldReturnSuccessWhenStaticSuccess()
        => Result.Success(1)
            .Assert(value => Assert.Equal(1, value.Value));

    [Fact]
    public void ShouldReturnUnitWhenStaticSuccess()
        => Result.Success()
            .Assert(value => Assert.IsType<Result>(value))
            .Assert(value => Assert.IsType<Unit>(value.Value));

    [Fact]
    public void ShouldReturnFailureWhenStaticFailure()
        => Result.Failure<int>(new Exception("Failure"))
            .Assert(value => Assert.Equal("Failure", value.Exception!.Message));

    [Fact]
    public void ShouldReturnSuccessWhenContinueIfSuccess()
        => Runs()
            .ContinueIf(_ => true)
            .Bind(_ => Runs("2"))
            .Assert(value => Assert.Equal("Success 2", value.Value));

    [Fact]
    public void ShouldReturnFailureWhenContinueIfNotSuccess()
        => Runs()
            .ContinueIf(_ => false, new Exception("Not Success"))
            .Bind(_ => Runs("2"))
            .Assert(value => Assert.Equal("Not Success", value.Exception!.Message));

    [Fact]
    public void ShouldReturnFailureWhenInitialFailureNotSuccess()
        => ReturnsError()
            .ContinueIf(_ => false, new Exception("Not Success"))
            .Bind(_ => Runs("2"))
            .Assert(value => Assert.Equal("Error", value.Exception!.Message));
}