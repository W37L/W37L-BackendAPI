using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.User.Entity;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Bases;
using W3TL.Core.Domain.Common.Values;
using W3TL.Core.Domain.Services;

public class User : AggregateRoot<UserID> {
    // Required for Reflection
    private User() : base(default!) { }

    internal User(UserID userId) : base(userId) { }

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

    public UserNameType UserName { get; internal set; }
    public NameType FirstName { get; internal set; }
    public LastNameType LastName { get; internal set; }
    public EmailType Email { get; internal set; }
    public PubType Pub { get; internal set; }
    public CreatedAtType CreatedAt { get; internal set; }
    public Profile Profile { get; internal set; }
    public List<PostId> Posts { get; internal set; } = new();
    public Interactions Interactions { get; internal set; } = new();


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

    public Result Follow(User user) {
        return FollowService.Handle(this, user);
    }

    public Result Unfollow(User user) {
        return UnFollowService.Handle(this, user);
    }

    // public Result Block(User user) {
    //     try {
    //         if (user == null) return Error.NullUser;
    //         if (Blocked.Contains(user.Id)) return Error.UserAlreadyBlocked;
    //         Blocked.Add(user.Id);
    //         return Result.Ok;
    //     }
    //     catch (Exception exception) {
    //         return Error.FromException(exception);
    //     }
    // }
    //
    // public Result Unblock(User user) {
    //     try {
    //         if (user == null) return Error.NullUser;
    //         if (!Blocked.Contains(user.Id)) return Error.UserNotBlocked;
    //         Blocked.Remove(user.Id);
    //         return Result.Ok;
    //     }
    //     catch (Exception exception) {
    //         return Error.FromException(exception);
    //     }
    // }
    //
    // public Result Mute(User user) {
    //     try {
    //         Muted.Add(user.Id);
    //         return Result.Ok;
    //     }
    //     catch (Exception exception) {
    //         return Error.FromException(exception);
    //     }
    // }
    //
    // public Result Unmute(User user) {
    //     try {
    //         Muted.Remove(user.Id);
    //         return Result.Ok;
    //     }
    //     catch (Exception exception) {
    //         return Error.FromException(exception);
    //     }
    // }
    //
    // public Result AddPost(Post post) {
    //     try {
    //         Posts.Add(post.Id as PostId);
    //         return Result.Ok;
    //     }
    //     catch (Exception exception) {
    //         return Error.FromException(exception);
    //     }
    // }
}