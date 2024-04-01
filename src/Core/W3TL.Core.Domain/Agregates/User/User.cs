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
        Likes = new List<Content>();
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
    public List<Content> Likes { get; private set; }

    public static Result<User> Create(UserNameType userName, NameType firstName, LastNameType lastName, EmailType email, PubType pub) {
        HashSet<Error> errors = new();

        // Validate inputs for null and ensure they're valid results
        if (userName == null) errors.Add(Error.BlankUserName);
        if (firstName == null) errors.Add(Error.InvalidName);
        if (lastName == null) errors.Add(Error.InvalidName);
        if (email == null) errors.Add(Error.InvalidEmail);
        if (pub == null) errors.Add(Error.InvalidPublicKeyFormat);

        // If there are any errors, return them without creating a User
        if (errors.Any()) return Error.CompileErrors(errors);

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
        if (userName is null) return Error.BlankUserName;
        try {
            UserName = userName;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateFirstName(NameType firstName) {
        if (firstName is null) return Error.InvalidName;
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

    public Result Block(User user) {
        try {
            if (user == null) return Error.NullUser;
            if (Blocked.Contains(user)) return Error.UserAlreadyBlocked;
            Blocked.Add(user);
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result Unblock(User user) {
        try {
            if (user == null) return Error.NullUser;
            if (!Blocked.Contains(user)) return Error.UserNotBlocked;
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