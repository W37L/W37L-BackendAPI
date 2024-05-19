using System.Text;

/**
 * Represents an error with a message and an optional chain to subsequent errors.
 * This class allows for creating a linked list of errors, enabling the aggregation of multiple error messages.
 */
public class Error {
    /// <summary>
    ///   The message associated with the error.
    /// </summary>
    private readonly string message;

    /// Represents an error message.
    /// @property {Error} nextError - The next error in the chain, if any.
    /// @constructor
    /// @param {string} message - The message associated with the error.
    /// @example
    /// Error error = new Error("An example error occurred.");
    /// /
    private Error nextError;

    /// The Error class represents an error with a specified message.
    /// Usage:
    /// Error exampleError = Error.ExampleError;
    /// string errorMessage = exampleError.Message;
    /// /
    private Error(string message) {
        this.message = message;
        this.nextError = null;
    }

    /// Gets the message associated with this error.
    /// /
    public string Message => this.message;

    /// Represents an error message along with a reference to the next error.
    /// /
    private Error Next => this.nextError;

    /// Represents an error.
    /// This class provides a set of static properties that represent common error scenarios. Each property represents a specific error and contains a message that describes the error scenario.
    /// /
    public static Error ExampleError => new Error("An example error occurred.");

    /// Represents an unknown error.
    /// /
    public static Error Unknown => new Error("An unknown error occurred.");

    /// Represents an error that occurs when a name contains invalid characters.
    /// This error is typically thrown when attempting to create or update a user's first or last name with a name
    /// that contains characters that are not allowed.
    /// Possible causes for this error include:
    /// - Using special characters or symbols in the name
    /// - Using numeric digits in the name
    /// - Using whitespace at the beginning or end of the name
    /// To fix this error, ensure that the name only contains valid alphabetic characters and does not have any leading or trailing whitespace.
    /// Possible resolutions for this error include:
    /// - Removing any invalid characters from the name
    /// - Checking for and trimming any leading or trailing whitespace in the name
    /// - Using a different name that does not contain any invalid characters
    /// Error Example:
    /// ```csharp
    /// var error = Error.InvalidName;
    /// Console.WriteLine(error.Message);
    /// ```
    /// This will output:
    /// ```
    /// The name contains invalid characters.
    /// ```
    /// /
    public static Error InvalidName => new Error("The name contains invalid characters.");

    /// Represents an error that occurs when an email address format is invalid.
    /// This error can be thrown when attempting to update the email of a user with an invalid email address format.
    /// Example usage:
    /// ```
    /// if (email is null)
    /// return Error.InvalidEmail;
    /// ```
    /// @see Error
    /// @see User
    /// @see EmailType
    /// /
    public static Error InvalidEmail => new Error("The email address format is invalid.");

    /// Represents an error that occurs when the Unix timestamp is invalid.
    /// This error is thrown when a Unix timestamp that is less than or equal to 0 is encountered.
    /// /
    public static Error InvalidUnixTime => new Error("The Unix timestamp is invalid.");

    /// Represents an error that occurs when the date format is invalid.
    /// /
    public static Error InvalidDateFormat => new Error("The date format is invalid.");

    /// Represents an error that occurs when the public key is blank.
    /// Usage examples:
    /// - In the PubType class, the BlankPublicKey error is returned when the public key is blank or whitespace.
    /// - The UserCreationTestFailure_WrongPubKey class has a test method that asserts the BlankPublicKey error is returned when creating a PubType object with a blank public key.
    /// /
    public static Error BlankPublicKey => new Error("The public key cannot be blank.");

    /// Represents an error indicating that the format of the public key is invalid.
    /// This error is typically raised when a public key string does not meet the expected format requirements.
    /// /
    public static Error InvalidPublicKeyFormat => new Error("The public key format is invalid.");

    /// Represents an error that occurs when the username value is blank or empty.
    /// /
    public static Error BlankUserName => new Error("The username cannot be blank.");

    /// Represents an error when the username format is invalid.
    /// /
    public static Error InvalidUserNameFormat => new Error("The username format is invalid.");

    /// Represents an error related to an invalid length of a value.
    /// This error occurs when the length of a value does not meet the expected length.
    /// Usage:
    /// Use InvalidLength to identify and handle cases where the length of a value is invalid.
    /// This error can be used in various classes and methods that require length validation.
    /// Example Usage:
    /// ```csharp
    /// var error = Error.InvalidLength;
    /// Console.WriteLine(error.Message);
    /// ```
    /// Associated Classes:
    /// - Error
    /// - ContentIDBase
    /// - UserID
    /// - CreateUserCommandtest
    /// - PostCreationTestFailure_WrongId
    /// /
    public static Error InvalidLength => new Error("The value length is invalid.");

    public static Error InvalidPrefix => new Error("The value does not start with the expected prefix.");

    /// Represents an error that occurs when an invalid URL format is encountered.
    /// /
    public static Error InvalidUrl => new Error("The URL format is invalid.");

    /// Represents a negative value error.
    /// This error occurs when a value is expected to be non-negative, but a negative value is provided.
    /// /
    public static Error NegativeValue => new Error("The value cannot be negative.");

    /// Determines if a user is blocked and prevents certain operations from being performed.
    /// /
    public static Error UserBlocked => new Error("The user is blocked, and the operation cannot be performed.");

    /// Represents an error when attempting to perform an operation on a user who is muted.
    /// /
    public static Error UserMuted => new Error("The user is muted, and the operation cannot be performed.");

    /// Represents an error indicating that the user is already followed.
    /// This error occurs when a user tries to follow another user, but the user is already being followed.
    /// The FollowService.Handle method checks if the follower is already following the followee, and if so,
    /// it adds the UserAlreadyFollowed error to the list of errors.
    /// This error is usually returned as a part of the Result object when handling a follow operation.
    /// Example usage:
    /// Result result = FollowService.Handle(follower, followee);
    /// if (result.IsSuccess) {
    /// // Follow operation successful
    /// } else {
    /// if (result.Error == Error.UserAlreadyFollowed) {
    /// // User is already followed
    /// } else {
    /// // Handle other errors
    /// }
    /// }
    /// /
    public static Error UserAlreadyFollowed =>
        new("The user is already followed, and the operation cannot be performed.");

    /// Represents an error that occurs when a user tries to follow themselves.
    /// /
    public static Error CannotFollowSelf => new Error("A user cannot follow themselves.");

    /// Represents an error that occurs when a null user is passed as a parameter.
    /// /
    public static Error NullUser => new Error("The user cannot be null.");

    /// UserNotFollowed
    /// Represents an error that occurs when a user is not followed, and a related operation cannot be performed.
    /// This error is typically returned in scenarios where a user is attempting to perform an action that requires a following relationship between two users, but the required relationship does not exist.
    /// This error is used in the following classes:
    /// - Error.cs
    /// - UnFollowService.cs
    /// - InMemInteractionRepoStub.cs
    /// - UserUnfollowTests.cs
    /// - UnfollowAUserHandlerTests.cs
    /// - InteractionRepository.cs
    /// /
    public static Error UserNotFollowed => new("The user is not followed, and the operation cannot be performed.");

    /// Represents an error indicating that the signature is invalid.
    /// /
    public static Error InvalidSignature =>
        new("The signature is invalid. It must be a 128-character hexadecimal or 88-character base64 string.");

    /// <summary>
    ///     Represents an error that occurs when the tweet content is null.
    /// </summary>
    public static Error NullContentTweet => new("The tweet content cannot be null.");

    /// NullCreator
    /// Represents an error when the creator parameter is null in the Post.Create method.
    /// This error indicates that the creator parameter cannot be null for creating a post.
    /// An instance of this error can be accessed through the NullCreator property of the Error class.
    /// Examples:
    /// ```csharp
    /// var postResult = Post.Create(contentResult, invalidCreator, signatureResult, PostType.Original);
    /// if (postResult.IsFailure)
    /// {
    /// var errors = postResult.Error.EnumerateAll().ToList();
    /// Assert.Contains(Error.NullCreator, errors);
    /// // Handle the error
    /// }
    /// ```
    /// /
    public static Error NullCreator => new("The creator cannot be null.");

    /// Represents an error that occurs when a null signature is passed as a parameter.
    /// /
    public static Error NullSignature => new("The signature cannot be null.");

    /// Represents an error that occurs when the post type is null.
    /// Possible causes for this error include:
    /// - The post type has not been specified.
    /// To fix this error, ensure that a valid post type is provided when creating a post.
    /// /
    public static Error NullPostType => new("The post type cannot be null.");

    /// Represents an error that occurs when a string is blank or null.
    /// This error is commonly used to validate string inputs for null or empty values.
    /// /
    public static Error BlankOrNullString => new("The string cannot be blank or null.");

    /// The `NullComment` error is thrown when a `null` value is passed as a `Comment` object.
    /// Possible Causes:
    /// - The comment was not provided or its value is `null`.
    /// Possible Solutions:
    /// - Ensure that a valid `Comment` object is passed as a parameter.
    /// /
    /// /
    public static Error NullComment => new("The comment cannot be null.");

    /// Gets the error that occurs when a string is null.
    /// /
    public static Error NullString => new("The string cannot be null.");

    /// Represents an error indicating that a string value cannot be blank.
    /// /
    public static Error BlankString => new("The string cannot be blank.");

    /// Represents an error that occurs when the email domain is invalid.
    /// This error typically occurs when a user inputs an email address with an invalid domain.
    /// The domain refers to the part of the email address after the "@" symbol.
    /// /
    public static Error InvalidEmailDomain => new("The email domain is invalid.");

    /// Represents an error that occurs when the media URL is null.
    /// /
    public static Error NullMediaUrl => new("The media URL cannot be null.");

    /// Represents an error related to a null content type.
    /// /
    public static Error NullContentType => new("The content type cannot be null.");

    /// Represents an error indicating that the parent post is null.
    /// /
    public static Error NullParentPost => new("The parent post cannot be null.");

    /// The UserAlreadyLiked property represents an error that occurs when a user tries to like a content that they have already liked.
    /// This property is defined in the Error class.
    /// It is used in the LikeService class to handle the liking of contents by users.
    /// The Handle method in the LikeService class checks if the user has already liked the content
    /// and adds the UserAlreadyLiked error to the list of errors if applicable.
    /// If there are any errors, it returns an Error object encapsulating the errors.
    /// The UserAlreadyLiked property is also tested in the LikeServiceTests class in the Like_AlreadyLikedContent_ReturnsError method.
    /// A test scenario is created where a user has already liked a content, and the method asserts that the UserAlreadyLiked error is returned.
    /// @see Error
    /// @see LikeService
    /// @see LikeServiceTests
    /// /
    public static Error UserAlreadyLiked => new("The user has already liked the post.");

    /// Represents the error that occurs when a user tries to unlike a content that they have not liked.
    /// This error is typically thrown by the `UnlikeService` class when handling the unlike operation.
    /// @see UnlikeService
    /// /
    public static Error UserNotLiked => new("The user has not liked the post.");

    /// Represents an error that occurs when trying to block a user, but the user is already blocked.
    /// This error is returned by the `Block` method in the `User` class.
    /// @see User.Block
    /// /
    public static Error UserAlreadyBlocked => new("The user is already blocked.");

    /// Indicates whether a user is not blocked.
    /// If a user is blocked, certain actions or operations may be restricted.
    /// If this value is true, it means the user is not blocked and the operation can proceed.
    /// If this value is false, it means the user is blocked and the operation cannot be performed.
    /// Examples:
    /// UserNotBlocked unblocked = Error.UserNotBlocked;
    /// Console.WriteLine(unblocked); // Output: "The user is not blocked."
    /// /
    /// /
    public static Error UserNotBlocked => new("The user is not blocked.");

    /// Represents an error that occurs when a user is already registered.
    /// /
    public static Error UserAlreadyRegistered => new("The user is already registered.");

    /// Represents an error indicating that the command is invalid.
    /// /
    public static Error InvalidCommand => new("The command is invalid.");

    /// Represents the error that occurs when a user is not found.
    /// /
    public static Error UserNotFound => new("The user was not found.");

    /// Represents an error indicating that the post type is invalid.
    /// /
    public static Error InvalidPostType => new("The post type is invalid.");

    /// Represents an error indicating that the media type is invalid.
    /// /
    public static Error InvalidMediaType => new("The media type is invalid.");

    /// Indicates that a post with the given ID is already registered.
    /// /
    public static Error PostAlreadyRegistered => new("The post is already registered.");

    /// Represents an error that occurs when the parent post is not found.
    /// /
    public static Error ParentPostNotFound => new("The parent post was not found.");

    /// Represents an error that occurs when content is not found.
    /// /
    public static Error ContentNotFound => new("The content was not found.");

    /// Represents an error that occurs when a post is not found.
    /// This error is used in the Highlight, Retweet, Unretweet, and UnHighlight classes.
    /// /
    public static Error PostNotFound => new("The post was not found.");

    /// Represents the error related to an incorrect number of parameters.
    /// /
    public static Error WrongNumberOfParameters => new("The number of parameters is incorrect.");

    /// Represents an error indicating that the value format is invalid.
    /// This error is typically encountered when validating a string value against a specific format.
    /// The format specified is a regex pattern: ^[a-zA-Z0-9]{28}$, which matches a 28-character alphanumeric string.
    /// If the value does not match this pattern, this error is thrown.
    /// This error can be used in various scenarios, such as validating user IDs, command arguments, etc.
    /// Example Usage:
    /// ```csharp
    /// var userIdInput = "Wrong-123"; // Example input that fails multiple validations
    /// var result = UserID.Create(userIdInput);
    /// Assert.True(result.IsFailure);
    /// Assert.Contains(Error.InvalidFormat, result.Error.EnumerateAll());
    /// ```
    /// @see [UserID](~/UserID.cs)
    /// @see [UserCreationTestFailure_WrongId](~/UserCreationTestFailure_WrongId.cs)
    /// @see [BlockUserCommandTests](~/BlockUserCommandTests.cs)
    /// @see [UnblockUserCommandTests](~/UnblockUserCommandTests.cs)
    /// @see [FollowAUserCommandTests](~/FollowAUserCommandTests.cs)
    /// @see [UnfollowAUserCommandTests](~/UnfollowAUserCommandTests.cs)
    /// /
    public static Error InvalidFormat => new("The value format is invalid.");

    /// Represents an error where the user is not verified.
    /// /
    public static Error NotVerified => new("The user is not verified.");

    /// Represents an error that occurs when a user attempts to like a tweet that has already been liked.
    /// /
    public static Error TweetAlreadyLiked => new("The tweet is already liked.");

    /// Represents an error when a tweet has not been liked.
    /// /
    public static Error TweetNotLiked => new("The tweet is not liked.");

    /// Represents an error that occurs when the provided email is already registered.
    /// Error.EmailAlreadyRegistered can be returned when attempting to create a new user with an email that is already in use.
    /// It is thrown by the CreateUserHandler.HandleAsync method when the user with the given email already exists in the repository.
    /// Example usage:
    /// var result = await userRepository.GetByEmailAsync(command.Email.Value);
    /// if (result.IsSuccess)
    /// return Error.EmailAlreadyRegistered;
    /// /
    public static Error EmailAlreadyRegistered => new("The email is already registered.");

    /// Indicates that the username is already registered.
    /// /
    public static Error UserNameAlreadyRegistered => new("The username is already registered.");

    /// Represents an unauthorized error.
    /// /
    public static Error UnAuthorized => new("Unauthorized, please login.");

    /// Represents an error that occurs when attempting to highlight a tweet that is already highlighted.
    /// Inherits from the `Error` class.
    /// /
    public static Error TweetAlreadyHighlighted => new("The tweet is already highlighted.");

    /// Represents an error that occurs when a tweet is not highlighted.
    /// /
    public static Error TweetNotHighlighted => new("The tweet is not highlighted.");

    /// Represents an error that occurs when a tweet is already retweeted.
    /// This error indicates that the tweet has already been retweeted by the user and the retweet operation cannot be performed.
    /// The `TweetAlreadyRetweeted` error is usually returned by the `InteractionRepository.RetweetAsync()` method in the `InteractionRepository` class.
    /// Example usage:
    /// ```
    /// if (!interactions.retweetedTweetIds.Contains(tweetId)) {
    /// return Error.TweetAlreadyRetweeted;
    /// }
    /// ```
    /// /
    public static Error TweetAlreadyRetweeted => new("The tweet is already retweeted.");

    /// Represents an error that occurs when trying to unretweet a tweet that has not been retweeted.
    /// /
    public static Error TweetNotRetweeted => new("The tweet is not retweeted.");

    /// Represents an error that occurs when a user is not reported.
    /// /
    public static Error UserNotReported => new("The user is not reported.");

    /// Represents an error that occurs when a user is already reported.
    /// Use this error when a user is being reported and they have already been reported previously.
    /// /
    public static Error UserAlreadyReported => new("The user is already reported.");

    /// <summary>
    ///     Represents an error that occurs when attempting to unmute a user who is not currently muted.
    /// </summary>
    public static Error UserNotMuted => new("The user is not muted.");

    /// Represents an error that occurs when trying to mute a user who is already muted.
    /// /
    public static Error UserAlreadyMuted => new("The user is already muted.");

    /// Represents an error that occurs when a user tries to follow themselves.
    /// This error is typically returned when the user ID in the request matches the ID of the user they are trying to follow.
    /// /
    public static Error CannotFollowYourself => new("A user cannot follow themselves.");

    /// Gets an error indicating that a user cannot block themselves.
    /// /
    public static Error CannotBlockYourself => new("A user cannot block themselves.");

    /// Error: CannotMuteYourself
    /// Description:
    /// - An error indicating that a user is trying to mute themselves.
    /// Remarks:
    /// - This error occurs when a user tries to mute themselves on the Mute endpoint.
    /// - Muting oneself is not allowed and will result in this error being returned.
    /// Possible Causes:
    /// - The user mistakenly sends a request to mute themselves.
    /// Resolution:
    /// - The user should ensure that they are not trying to mute themselves and send the request again.
    public static Error CannotMuteYourself => new("A user cannot mute themselves.");

    /// This error occurs when a user tries to report themselves.
    /// /
    public static Error CannotReportYourself => new("A user cannot report themselves.");

    /// Represents an error that occurs when a user attempts to unblock themselves.
    /// /
    public static Error CannotUnblockYourself => new("A user cannot unblock themselves.");

    /// Represents an error that occurs when a user tries to unfollow themselves.
    /// /
    /// /
    public static Error CannotUnfollowYourself => new("A user cannot unfollow themselves.");

    /// <summary>
    ///     Represents the error that occurs when a user tries to unmute themselves.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         To resolve this error, the user should attempt to unmute another user instead of themselves.
    ///     </para>
    /// </remarks>
    /// /
    public static Error CannotUnmuteYourself => new("A user cannot unmute themselves.");

    /// Represents an error that occurs when a user tries to unreport themselves.
    /// This error indicates that a user cannot unreport themselves.
    /// This error is thrown in the `Unreport.HandleAsync` method of the `Unreport` class in the `Unreport.cs` file.
    /// It is thrown when the `userId` parameter is equal to the `userToUnreportId` parameter.
    /// Possible error messages:
    /// - "A user cannot unreport themselves."
    /// Example usage:
    /// ```
    /// var error = Error.CannotUnreportYourself;
    /// Console.WriteLine(error.Message); // "A user cannot unreport themselves."
    /// ```
    /// /
    public static Error CannotUnreportYourself => new("A user cannot unreport themselves.");

    /// Represents an error that occurs when attempting to update a field with an invalid value.
    /// /
    public static Error InvalidField => new("The field is invalid.");

    /// Represents an error that occurs when content is not liked.
    /// /
    public static Error ContentNotLiked => new("The content is not liked.");

    /// Represents an error message indicating that the content is not retweeted.
    /// /
    public static Error ContentNotRetweeted => new("The content is not retweeted.");

    /// Represents an error indicating that the content is not highlighted.
    /// /
    public static Error ContentNotHighlighted => new("The content is not highlighted.");

    /// Returns an Error object indicating that the user is already in the specified list.
    /// @param listName The name of the list.
    /// @returns An Error object representing the user already being in the specified list.
    /// /
    public static Error UserAlreadyInList(string listName) => new($"The user is already in the {listName} list.");

    /// Creates a new instance of the UserNotActioned Error with the specified list name.
    /// @param listName The name of the list the user is not actioned in.
    /// @returns An instance of the UserNotActioned Error.
    /// /
    public static Error UserNotActioned(string listName) => new($"The user is not in the {listName} list.");

    /// <summary>
    ///     Initializes a new instance of the Error class with a specified message.
    /// </summary>
    /// <param name="message">The message describing the error.</param>
    /// <returns>An instance of the Error class.</returns>
    public static Error FromString(string message) => new Error(message);

    /// Generates an `Error` object indicating that a specified property does not have a setter.
    /// @param propertyName The name of the property that does not have a setter.
    /// @returns An `Error` object indicating that the property does not have a setter.
    /// /
    public static Error PropertyDoesNotHaveSetter(string propertyName) =>
        new Error($"Property '{propertyName}' does not have a setter.");

    /// Throws an instance of the Error class with a specified message indicating that a property with the given name does not exist.
    /// @param propertyName The name of the property that does not exist.
    /// @returns An instance of the Error class indicating that the property does not exist.
    /// /
    public static Error PropertyDoesNotExist(string propertyName) =>
        new Error($"Property '{propertyName}' does not exist.");

    /// Creates an instance of the Error class with a specified message indicating a string that is too long.
    /// @param maxLength The maximum allowed length for the string.
    /// @returns An instance of the Error class indicating that the string exceeds the maximum allowed length.
    /// /
    public static Error TooLongString(int maxLength) => new Error($"The string cannot exceed {maxLength} characters.");

    /// Generates an instance of the Error class with a customized message indicating that the name must be at least a specific minimum length.
    /// @param minLength The minimum length that the name must have.
    /// @returns An Error instance with a message indicating that the name must be at least minLength characters long.
    /// /
    public static Error TooShortName(int minLength) =>
        new Error($"The name must be at least {minLength} characters long.");

    /// Initializes a new instance of the TooLongName class with a specified maximum length.
    /// @param maxLength The maximum length of the name.
    /// @returns A new instance of the Error class representing a name that exceeds the maximum length.
    /// /
    public static Error TooLongName(int maxLength) => new Error($"The name cannot exceed {maxLength} characters.");

    /// Creates a new instance of the Error class from an exception.
    /// @param exception The exception from which to create the error.
    /// @returns An instance of the Error class with the same message as the exception.
    /// /
    public static Error FromException(Exception exception) => new Error(exception.Message);

    /// Initializes a new instance of the Error class with a specified message.
    /// @param maxLength The maximum length allowed for the biography.
    /// @returns An instance of Error indicating that the biography cannot exceed the specified maximum length.
    /// Example usage:
    /// var error = Error.TooLongBio(500);
    /// /
    public static Error TooLongBio(int maxLength) => new Error($"The biography cannot exceed {maxLength} characters.");

    /// Defines an error indicating that the location exceeds the maximum allowed length.
    /// @param maxLength The maximum length allowed for the location.
    /// @returns The error indicating that the location is too long.
    /// /
    public static Error TooLongLocation(int maxLength) =>
        new Error($"The location cannot exceed {maxLength} characters.");


    /// Attaches a new error to the end of the chain. If the chain has subsequent errors,
    /// it navigates to the end of the chain before attaching the new error.
    /// @param newError The error to be attached to the chain.
    /// /
    public void Attach(Error newError) {
        Error current = this;
        while (current.nextError != null) {
            current = current.nextError;
        }

        current.nextError = newError;
    }

    /// Compiles a set of errors into a single chained error. This method initializes
    /// a new error chain and appends each error from the set into the chain.
    /// @param errors The collection of errors to compile into the chain.
    /// @return The head of the newly formed error chain, excluding the initial placeholder error.
    /// /
    public static Error CompileErrors(HashSet<Error> errors) {
        Error rootError = new Error("Compiled Errors");
        foreach (var error in errors) {
            rootError.Attach(error);
        }

        return rootError.nextError; // Skip the root placeholder error.
    }

    /// Enumerates all errors in the chain starting from this instance. This allows for iterating
    /// through each error in the chain.
    /// @return An enumerable of all errors in the chain.
    /// /
    public IEnumerable<Error> EnumerateAll() {
        for (Error e = this; e != null; e = e.nextError) {
            yield return e;
        }
    }

    /// Returns a string that represents the current error and any subsequent errors in the chain.
    /// Each error message is separated by a newline character.
    /// @return A string representation of the error chain.
    /// /
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

    /// Determines whether the specified object is equal to the current error, based on the message.
    /// @param obj The object to compare with the current object.
    /// @return true if the specified object is an error with the same message; otherwise, false.
    /// /
    public override bool Equals(object? obj) {
        return obj is Error error && this.message == error.message;
    }

    /// Serves as the default hash function, hashing the error message.
    /// @return A hash code for the current object.
    /// /
    public override int GetHashCode() {
        return HashCode.Combine(message);
    }
}