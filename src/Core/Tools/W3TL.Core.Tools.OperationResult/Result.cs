/// Represents the outcome of an operation, indicating success or failure.
/// It contains a utility method for chaining further actions based on the result.
/// /
public class Result {
    /// Represents the outcome of an operation, indicating success or failure.
    /// It contains a utility method for chaining further actions based on the result.
    /// /
    public static readonly Result Ok = new(true, null!);

    /// Represents the outcome of an operation, indicating success or failure.
    /// It contains a utility method for chaining further actions based on the result.
    /// /
    protected Result(bool isSuccess, Error error) {
        IsSuccess = isSuccess;
        Error = error;
    }

    /// Gets a value indicating whether the operation represented by the Result object was successful.
    /// @return true if the operation was successful; otherwise, false.
    /// /
    public bool IsSuccess { get; }

    /// Represents an error that can occur during an operation.
    /// /
    public Error Error { get; }

    /// Gets a value indicating whether the result represents a failure.
    /// /
    public bool IsFailure => !IsSuccess;

    /// Creates a failure result with the specified error.
    /// @param error The error to be associated with this result.
    /// @return A Result indicating failure.
    /// /
    public static Result Fail(Error error) {
        return new Result(false, error);
    }

    /// Retrieves a static instance representing a successful result.
    /// @return A Result indicating success.
    /// /
    public static Result Success() {
        return Ok;
    }

    /// Represents the outcome of an operation, indicating success or failure.
    /// It contains utility methods for chaining further actions based on the result.
    /// /
    public static implicit operator Result(Error error) {
        return Fail(error);
    }

    /// Implicitly converts a boolean value to a `Result` object.
    /// If the `successFlag` is `true`, returns a `Result` representing success. Otherwise, returns a `Result` representing failure with an `Error.Unknown`.
    /// @param successFlag The boolean value indicating the success or failure of the operation.
    /// @return A `Result` object representing the success or failure of the operation.
    /// /
    public static implicit operator Result(bool successFlag) {
        return successFlag ? Success() : Fail(Error.Unknown);
    }

    /// Executes a given action if the result is successful.
    /// @param action The action to perform on success.
    /// @return The current Result instance for method chaining.
    /// /
    public Result OnSuccess(Action action) {
        if (IsSuccess) {
            action?.Invoke();
        }

        return this;
    }

    /// Executes a given action if the result is a failure, providing the associated error.
    /// @param action The action to perform on failure.
    /// @return The current Result instance for method chaining.
    /// /
    public Result OnFailure(Action<Error> action) {
        if (IsFailure) {
            action?.Invoke(this.Error);
        }

        return this;
    }
}

/// Represents the outcome of an operation that returns a value on success.
/// It extends the Result class, adding functionality to handle success values.
/// @typeparam T The type of the value associated with a successful outcome.
/// /
public class Result<T> : Result {
    /// Represents the outcome of an operation that returns a value on success.
    /// It extends the Result class, adding functionality to handle success values.
    /// @typeparam T The type of the value associated with a successful outcome.
    /// /
    private Result(bool isSuccess, T value, Error error) : base(isSuccess, error) {
        Payload = value;
    }

    /// <summary>
    ///     Represents the payload of a Result object.
    /// </summary>
    /// <typeparam name="T">The type of the payload.</typeparam>
    /// <remarks>
    ///     The Payload property contains the value associated with a successful Result.
    /// </remarks>
    public T Payload { get; }

    /// Creates a successful Result indicating success.
    /// @param value The success value.
    /// @return A Result indicating success.
    /// /
    public static Result<T> Ok(T value) {
        return new Result<T>(true, value, null);
    }

    /// Creates a successful Result holding the specified value.
    /// @param value The success value.
    /// @return A Result indicating success with the associated value.
    /// /
    public static Result<T> Success(T value) {
        return Ok(value);
    }

    /// Creates a failed Result for the specified type, with the given error.
    /// @param error The error to be associated with this result.
    /// @return A Result indicating failure.
    /// /
    public static Result<T> Fail(Error error) {
        return new Result<T>(false, default(T), error);
    }

    /// Represents the outcome of an operation that returns a value on success.
    /// It extends the Result class, adding functionality to handle success values.
    /// @typeparam T The type of the value associated with a successful outcome.
    /// /
    public static implicit operator Result<T>(T value) {
        return Ok(value);
    }

    /// Represents the operator used to define custom implicit conversions in the `Result
    /// <T>
    ///     ` class.
    ///     @param
    ///     <T>
    ///         The type of the value associated with a successful outcome.
    ///         @param error The error object used to create a failed result.
    ///         @return The corresponding failed result with the specified error.
    ///         /
    public static implicit operator Result<T>(Error error) {
        return Fail(error);
    }

    /// Executes a given action if the result is successful, providing the success value.
    /// @param action The action to perform on success, receiving the success value.
    /// @return The current Result
    /// <T>
    ///     instance for method chaining.
    ///     /
    public new Result<T> OnSuccess(Action<T> action) {
        if (IsSuccess) {
            action?.Invoke(Payload);
        }

        return this;
    }

    /// Executes a given action if the result is a failure, providing the associated error.
    /// @param action The action to perform on failure, receiving the associated error.
    /// @return The current Result
    /// <T>
    ///     instance for method chaining.
    ///     /
    public new Result<T> OnFailure(Action<Error> action) {
        if (IsFailure) {
            action?.Invoke(this.Error);
        }

        return this;
    }
}