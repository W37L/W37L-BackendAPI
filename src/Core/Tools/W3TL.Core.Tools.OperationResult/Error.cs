using System.Text;

/**
 * Represents an error with a message and an optional chain to subsequent errors.
 * This class allows for creating a linked list of errors, enabling the aggregation of multiple error messages.
 */
public class Error {
    private readonly string message;
    private Error nextError;

    /**
     * Initializes a new instance of the Error class with a specified message.
     *
     * @param message The message describing the error.
     */
    private Error(string message) {
        this.message = message;
        this.nextError = null;
    }

    /**
     * Gets the message associated with this error.
     */
    public string Message => this.message;

    /**
     * Gets the next error in the chain, if any.
     */
    private Error Next => this.nextError;

    /**
     * These are predefined error instances that can be used to represent common error scenarios.
     */
    public static Error ExampleError => new Error("An example error occurred.");

    public static Error Unknown => new Error("An unknown error occurred.");
    public static Error InvalidName => new Error("The name contains invalid characters.");
    public static Error InvalidEmail => new Error("The email address format is invalid.");
    public static Error InvalidUnixTime => new Error("The Unix timestamp is invalid.");
    public static Error InvalidDateFormat => new Error("The date format is invalid.");
    public static Error BlankPublicKey => new Error("The public key cannot be blank.");
    public static Error InvalidPublicKeyFormat => new Error("The public key format is invalid.");
    public static Error BlankUserName => new Error("The username cannot be blank.");
    public static Error InvalidUserNameFormat => new Error("The username format is invalid.");
    public static Error InvalidLength => new Error("The value length is invalid.");
    public static Error InvalidPrefix => new Error("The value does not start with the expected prefix.");
    public static Error InvalidUrl => new Error("The URL format is invalid.");
    public static Error NegativeValue => new Error("The value cannot be negative.");
    public static Error UserBlocked => new Error("The user is blocked, and the operation cannot be performed.");
    public static Error UserMuted => new Error("The user is muted, and the operation cannot be performed.");

    public static Error UserAlreadyFollowed =>
        new("The user is already followed, and the operation cannot be performed.");

    public static Error CannotFollowSelf => new Error("A user cannot follow themselves.");
    public static Error NullUser => new Error("The user cannot be null.");

    public static Error UserNotFollowed => new("The user is not followed, and the operation cannot be performed.");

    public static Error InvalidSignature =>
        new("The signature is invalid. It must be a 128-character hexadecimal or 88-character base64 string.");

    public static Error NullContentTweet => new("The tweet content cannot be null.");
    public static Error NullCreator => new("The creator cannot be null.");
    public static Error NullSignature => new("The signature cannot be null.");
    public static Error NullPostType => new("The post type cannot be null.");
    public static Error BlankOrNullString => new("The string cannot be blank or null.");
    public static Error NullComment => new("The comment cannot be null.");
    public static Error NullString => new("The string cannot be null.");
    public static Error BlankString => new("The string cannot be blank.");
    public static Error InvalidEmailDomain => new("The email domain is invalid.");
    public static Error NullMediaUrl => new("The media URL cannot be null.");
    public static Error NullContentType => new("The content type cannot be null.");
    public static Error NullParentPost => new("The parent post cannot be null.");
    public static Error UserAlreadyLiked => new("The user has already liked the post.");
    public static Error UserNotLiked => new("The user has not liked the post.");
    public static Error UserAlreadyBlocked => new("The user is already blocked.");
    public static Error UserNotBlocked => new("The user is not blocked.");
    public static Error UserAlreadyRegistered => new("The user is already registered.");
    public static Error InvalidCommand => new("The command is invalid.");
    public static Error UserNotFound => new("The user was not found.");
    public static Error InvalidPostType => new("The post type is invalid.");
    public static Error InvalidMediaType => new("The media type is invalid.");
    public static Error PostAlreadyRegistered => new("The post is already registered.");
    public static Error ParentPostNotFound => new("The parent post was not found.");
    public static Error ContentNotFound => new("The content was not found.");
    public static Error PostNotFound => new("The post was not found.");
    public static Error WrongNumberOfParameters => new("The number of parameters is incorrect.");
    public static Error InvalidFormat => new("The value format is invalid.");
    public static Error NotVerified => new("The user is not verified.");
    public static Error TweetAlreadyLiked => new("The tweet is already liked.");
    public static Error TweetNotLiked => new("The tweet is not liked.");


    public static Error FromString(string message) {
        return new Error(message);
    }

    public static Error TooLongString(int maxLength) => new Error($"The string cannot exceed {maxLength} characters.");

    public static Error TooShortName(int minLength) {
        return new Error($"The name must be at least {minLength} characters long.");
    }

    public static Error TooLongName(int maxLength) => new Error($"The name cannot exceed {maxLength} characters.");
    public static Error FromException(Exception exception) => new Error(exception.Message);
    public static Error TooLongBio(int maxLength) => new Error($"The biography cannot exceed {maxLength} characters.");

    public static Error TooLongLocation(int maxLength) {
        return new Error($"The location cannot exceed {maxLength} characters.");
    }


    /**
     * Attaches a new error to the end of the chain. If the chain has subsequent errors,
     * it navigates to the end of the chain before attaching the new error.
     *
     * @param newError The error to be attached to the chain.
     */
    public void Attach(Error newError) {
        Error current = this;
        while (current.nextError != null) {
            current = current.nextError;
        }

        current.nextError = newError;
    }

    /**
     * Compiles a set of errors into a single chained error. This method initializes
     * a new error chain and appends each error from the set into the chain.
     *
     * @param errors The collection of errors to compile into the chain.
     * @return The head of the newly formed error chain, excluding the initial placeholder error.
     */
    public static Error CompileErrors(HashSet<Error> errors) {
        Error rootError = new Error("Compiled Errors");
        foreach (var error in errors) {
            rootError.Attach(error);
        }

        return rootError.nextError; // Skip the root placeholder error.
    }

    /**
     * Enumerates all errors in the chain starting from this instance. This allows for iterating
     * through each error in the chain.
     *
     * @return An enumerable of all errors in the chain.
     */
    public IEnumerable<Error> EnumerateAll() {
        for (Error e = this; e != null; e = e.nextError) {
            yield return e;
        }
    }

    /**
     * Returns a string that represents the current error and any subsequent errors in the chain.
     * Each error message is separated by a newline character.
     *
     * @return A string representation of the error chain.
     */
    public override string ToString() {
        var builder = new StringBuilder();
        builder.Append(this.Message);

        Error current = this.nextError;
        while (current != null) {
            builder.AppendLine();
            builder.Append(current.Message);
            current = current.Next;
        }

        return builder.ToString();
    }

    /**
     * Determines whether the specified object is equal to the current error, based on the message.
     *
     * @param obj The object to compare with the current object.
     * @return true if the specified object is an error with the same message; otherwise, false.
     */
    public override bool Equals(object? obj) {
        return obj is Error error && this.message == error.message;
    }

    /**
     * Serves as the default hash function, hashing the error message.
     *
     * @return A hash code for the current object.
     */
    public override int GetHashCode() {
        return HashCode.Combine(message);
    }
}