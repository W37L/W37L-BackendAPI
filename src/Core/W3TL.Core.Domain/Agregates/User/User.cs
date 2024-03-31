using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.User.Entity;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Bases;
using W3TL.Core.Domain.Common.Values;
using W3TL.Core.Domain.Services;

public class User : AggregateRoot<UserID> {
    internal User(UserID userId) : base(userId) { }

    private User(
        UserID userId,
        UserNameType userName,
        NameType firstName,
        LastNameType lastName,
        EmailType email,
        PubType pub,
        CreatedAtType createdAt,
        Profile profile
    ) : base(userId) {
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Pub = pub;
        CreatedAt = createdAt;
        Profile = profile;
        Followers = new List<User>();
        Following = new List<User>();
        Blocked = new List<User>();
        Muted = new List<User>();
        Posts = new List<Post>();
    }

    public UserNameType UserName { get; internal set; }
    public NameType FirstName { get; internal set; }
    public LastNameType LastName { get; internal set; }
    public EmailType Email { get; internal set; }
    public PubType Pub { get; internal set; }
    public CreatedAtType CreatedAt { get; internal set; }
    public Profile Profile { get; internal set; }

    public List<User> Followers { get; private set; }
    public List<User> Following { get; private set; }
    public List<User> Blocked { get; private set; }
    public List<User> Muted { get; private set; }

    public List<Post> Posts { get; private set; }

    public static Result<User> Create(UserNameType userName, NameType firstName, LastNameType lastName, EmailType email, PubType pub) {
        try {
            var userId = UserID.Generate().Payload;
            var profile = Profile.Create(userId).Payload;
            var createdAt = CreatedAtType.Create().Payload;
            var user = new User(userId, userName, firstName, lastName, email, pub, createdAt, profile);
            return user;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateUserName(UserNameType userName) {
        try {
            UserName = userName;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateFirstName(NameType firstName) {
        try {
            FirstName = firstName;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateLastName(LastNameType lastName) {
        try {
            LastName = lastName;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateEmail(EmailType email) {
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

    public Result Block(User user) {
        try {
            Blocked.Add(user);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result Unblock(User user) {
        try {
            Blocked.Remove(user);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result Mute(User user) {
        try {
            Muted.Add(user);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result Unmute(User user) {
        try {
            Muted.Remove(user);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result AddPost(Post post) {
        try {
            Posts.Add(post);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}