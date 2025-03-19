using EventAssociation.Core.Tools.OperationResult;

namespace UnitTests.OperationResult;

public class OperationResultUnitTest
{
    public class ResultTests
    {
        [Fact]
        public void Success_ShouldCreateSuccessfulResult()
        {
            var result = Results.Success();

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Failure_ShouldCreateFailedResultWithErrors()
        {
            var error = new Error("ERR001", "An error occurred");
            var result = Results.Failure(error);

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Single(result.Errors);
            Assert.Equal("ERR001", result.Errors[0].Code);
            Assert.Equal("An error occurred", result.Errors[0].Message);
        }

        [Fact]
        public void Success_WithPayload_ShouldCreateSuccessfulResultWithValue()
        {
            var result = Results<int>.Success(42);

            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Equal(42, result.Value);
        }

        [Fact]
        public void Failure_WithPayload_ShouldCreateFailedResultWithErrors()
        {
            var error = new Error("ERR002", "Another error occurred");
            var result = Results<int>.Failure(error);

            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Single(result.Errors);
            Assert.Equal("ERR002", result.Errors[0].Code);
        }

        [Fact]
        public void Combine_WithOnlySuccessfulResults_ShouldReturnSuccess()
        {
            var result1 = Results.Success();
            var result2 = Results.Success();
            var combined = Results.Combine(result1, result2);

            Assert.True(combined.IsSuccess);
            Assert.Empty(combined.Errors);
        }

        [Fact]
        public void Combine_WithFailedResults_ShouldReturnFailureWithAllErrors()
        {
            var error1 = new Error("ERR003", "First error");
            var error2 = new Error("ERR004", "Second error");

            var result1 = Results.Failure(error1);
            var result2 = Results.Failure(error2);

            var combined = Results.Combine(result1, result2);

            Assert.False(combined.IsSuccess);
            Assert.Equal(2, combined.Errors.Count);
            Assert.Contains(combined.Errors, e => e.Code == "ERR003");
            Assert.Contains(combined.Errors, e => e.Code == "ERR004");
        }

        [Fact]
        public void ImplicitConversion_ShouldCreateSuccessfulResultFromValue()
        {
            Results<int> result = 100;

            Assert.True(result.IsSuccess);
            Assert.Equal(100, result.Value);
        }
    }
}