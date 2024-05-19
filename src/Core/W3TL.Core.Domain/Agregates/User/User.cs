using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.User.Entity;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Bases;
using W3TL.Core.Domain.Common.Values;
using W3TL.Core.Domain.Services;

/// <summary>
/// Represents a user entity within the system, extending the AggregateRoot class with a UserID.
/// </summary>
public class User : AggregateRoot<UserID> {
    // Required for Reflection
    private User() : base(default!) { }

    // Required for Factory
    internal User(UserID userId) : base(userId) {
        Profile = Profile.Create(userId).Payload;
        Interactions = Interactions.Create(userId).Payload;
    }

    private User(
        UserID userId,
        UserNameType userName,
        NameType firstName,
        LastNameType lastName,
        EmailType email,
        PubType pub,
        CreatedAtType createdAt,
        Profile profile,
        Interactions interactions
    ) : base(userId) {
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Pub = pub;
        CreatedAt = createdAt;
        Profile = profile;
        Interactions = interactions;
    }

    /// <summary>
    /// Represents the username of the user.
    /// </summary>
    public UserNameType UserName { get; internal set; }

    /// <summary>
    /// Represents the first name of the user.
    /// </summary>
    public NameType FirstName { get; internal set; }

    /// <summary>
    /// Represents the last name of the user.
    /// </summary>
    public LastNameType LastName { get; internal set; }

    /// <summary>
    /// Represents the email address of the user.
    /// </summary>
    public EmailType Email { get; internal set; }

    /// <summary>
    /// Represents the public key of the user.
    /// </summary>
    public PubType Pub { get; internal set; }

    /// <summary>
    /// Represents the creation date of the user's account.
    /// </summary>
    public CreatedAtType CreatedAt { get; internal set; }

    /// <summary>
    /// Represents the profile information of the user.
    /// </summary>
    public Profile Profile { get; internal set; }

    /// <summary>
    /// Represents the list of posts made by the user.
    /// </summary>
    public List<PostId> Posts { get; internal set; } = new();

    /// <summary>
    /// Represents the interactions of the user.
    /// </summary>
    public Interactions Interactions { get; set; }

    /// <summary>
    /// Creates a new user with the provided details.
    /// </summary>
    public static Result<User> Create(UserID userId, UserNameType userName, NameType firstName, LastNameType lastName,
        EmailType email, PubType pub) {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(userName);
        ArgumentNullException.ThrowIfNull(firstName);
        ArgumentNullException.ThrowIfNull(lastName);
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(pub);
        try {
            var profile = Profile.Create(userId).Payload;
            var createdAt = CreatedAtType.Create().Payload!;
            var interactions = Interactions.Create(userId).Payload;
            var user = new User(userId, userName, firstName, lastName, email, pub, createdAt, profile, interactions);
            return user;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Updates the username of the user.
    /// </summary>
    public Result UpdateUserName(UserNameType userName) {
        if (userName is null) throw new ArgumentNullException(nameof(userName));
        try {
            UserName = userName;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Updates the first name of the user.
    /// </summary>
    public Result UpdateFirstName(NameType firstName) {
        if (firstName is null) throw new ArgumentNullException(nameof(firstName));
        try {
            FirstName = firstName;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Updates the last name of the user.
    /// </summary>
    public Result UpdateLastName(LastNameType lastName) {
        if (lastName is null) return Error.InvalidName;
        try {
            LastName = lastName;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Updates the email address of the user.
    /// </summary>
    public Result UpdateEmail(EmailType email) {
        if (email is null) return Error.InvalidEmail;
        try {
            Email = email;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Follows another user.
    /// </summary>
    public Result Follow(User user) {
        return FollowService.Handle(this, user);
    }

    /// <summary>
    /// Unfollows another user.
    /// </summary>
    public Result Unfollow(User user) {
        return UnFollowService.Handle(this, user);
    }

    /// <summary>
    /// Blocks another user.
    /// </summary>
    public Result Block(User user) {
        try {
            if (user == null) return Error.NullUser;
            if (Interactions.Blocked.Contains(user.Id)) return Error.UserAlreadyBlocked;
            Interactions.Blocked.Add(user.Id);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Unblocks a previously blocked user.
    /// </summary>
    public Result Unblock(User user) {
        try {
            if (user == null) return Error.NullUser;
            if (!Interactions.Blocked.Contains(user.Id)) return Error.UserNotBlocked;
            Interactions.Blocked.Remove(user.Id);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Mutes another user.
    /// </summary>
    public Result Mute(User user) {
        try {
            Interactions.Muted.Add(user.Id);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Unmutes a previously muted user.
    /// </summary>
    public Result Unmute(User user) {
        try {
            Interactions.Muted.Remove(user.Id);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Reports another user.
    /// </summary>
    public Result Report(User user) {
        try {
            Interactions.ReportedUsers.Add(user.Id);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    /// <summary>
    /// Unreports a previously reported user.
    /// </summary>
    public Result Unreport(User user) {
        try {
            Interactions.ReportedUsers.Remove(user.Id);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}