/**
 * Represents the outcome of an operation, indicating success or failure.
 * It contains a utility method for chaining further actions based on the result.
 */
public class Result {
    public static readonly Result Ok = new(true, null!);

    /**
     * Constructs a Result.
     * @param isSuccess Indicates if the result represents a success.
     * @param error The error associated with a failure.
     */
    protected Result(bool isSuccess, Error error) {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public Error Error { get; }
    public bool IsFailure => !IsSuccess;

    /**
     * Creates a failure result with the specified error.
     * @param error The error to be associated with this result.
     * @return A Result indicating failure.
     */
    public static Result Fail(Error error) {
        return new Result(false, error);
    }

    /**
     * Retrieves a static instance representing a successful result.
     * @return A Result indicating success.
     */
    public static Result Success() {
        return Ok;
    }

    public static implicit operator Result(Error error) {
        return Fail(error);
    }

    public static implicit operator Result(bool successFlag) {
        return successFlag ? Success() : Fail(Error.Unknown);
    }

    /**
     * Executes a given action if the result is successful.
     * @param action The action to perform on success.
     * @return The current Result instance for method chaining.
     */
    public Result OnSuccess(Action action) {
        if (IsSuccess) {
            action?.Invoke();
        }

        return this;
    }

    /**
     * Executes a given action if the result is a failure, providing the associated error.
     * @param action The action to perform on failure.
     * @return The current Result instance for method chaining.
     */
    public Result OnFailure(Action<Error> action) {
        if (IsFailure) {
            action?.Invoke(this.Error);
        }

        return this;
    }
}

/**
 * Represents the outcome of an operation that returns a value on success.
 * It extends the Result class, adding functionality to handle success values.
 * @param <T> The type of the value associated with a successful outcome.
 */
public class Result<T> : Result {
    /**
     * Constructs a Result with the specified success state, value, and error.
     * @param isSuccess Indicates if the result represents a success.
     * @param value The value associated with a successful result.
     * @param error The error associated with a failure.
     */
    private Result(bool isSuccess, T value, Error error) : base(isSuccess, error) {
        Payload = value;
    }

    public T Payload { get; }

    /**
     * Creates a successful Result holding the specified value.
     * @param value The success value.
     * @return A Result indicating success with the associated value.
     */
    public static Result<T> Ok(T value) {
        return new Result<T>(true, value, null);
    }

    /**
     * Creates a failed Result for the specified type, with the given error.
     * @param error The error to be associated with this result.
     * @return A Result indicating failure.
     */
    public static Result<T> Fail(Error error) {
        return new Result<T>(false, default(T), error);
    }

    public static implicit operator Result<T>(T value) {
        return Ok(value);
    }

    public static implicit operator Result<T>(Error error) {
        return Fail(error);
    }

    /**
     * Executes a given action if the result is successful, providing the success value.
     * @param action The action to perform on success, receiving the success value.
     * @return The current Result<T> instance for method chaining.
     */
    public new Result<T> OnSuccess(Action<T> action) {
        if (IsSuccess) {
            action?.Invoke(Payload);
        }

        return this;
    }

    /**
     * Executes a given action if the result is a failure, providing the associated error.
     * @param action The action to perform on failure, receiving the associated error.
     * @return The current Result<T> instance for method chaining.
     */
    public new Result<T> OnFailure(Action<Error> action) {
        if (IsFailure) {
            action?.Invoke(this.Error);
        }

        return this;
    }
}