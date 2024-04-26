using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.Post;
using W3TL.Core.Domain.Agregates.User.Entity;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Bases;
using W3TL.Core.Domain.Common.Values;
using W3TL.Core.Domain.Services;

public class User : AggregateRoot<UserID> {
    // Required for Reflection
    private User() : base(default!) { }

    internal User(UserID userId) : base(userId) {
        Followers = new List<User>();
        Following = new List<User>();
        Blocked = new List<User>();
        Muted = new List<User>();
        Posts = new List<Post>();
        Likes = new List<Content>();
        Highlights = new List<Content>();
        ReportedUsers = new List<User>();
        RetweetedTweets = new List<Post>();
    }

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
        Highlights = new List<Content>();
        Posts = new List<Post>();
        Likes = new List<Content>();
        ReportedUsers = new List<User>();
        RetweetedTweets = new List<Post>();
    }

    public UserNameType UserName { get; internal set; }
    public NameType FirstName { get; internal set; }
    public LastNameType LastName { get; internal set; }
    public EmailType Email { get; internal set; }
    public PubType Pub { get; internal set; }
    public CreatedAtType CreatedAt { get; internal set; }
    public Profile Profile { get; internal set; }
    public List<Content> Highlights { get; private set; }
    public List<User> ReportedUsers { get; private set; }
    public List<Post> RetweetedTweets { get; private set; }

    public List<User> Followers { get; private set; }
    public List<User> Following { get; private set; }
    public List<User> Blocked { get; private set; }
    public List<User> Muted { get; private set; }

    public List<Post> Posts { get; private set; }
    public List<Content> Likes { get; private set; }

    public static Result<User> Create(UserNameType userName, NameType firstName, LastNameType lastName, EmailType email,
        PubType pub) {
        var result = UserID.Generate();
        if (result.IsFailure) return result.Error;
        return Create(result.Payload, userName, firstName, lastName, email, pub);
    }

    public static Result<User> Create(UserID userId, UserNameType userName, NameType firstName, LastNameType lastName,
        EmailType email, PubType pub) {
        if (userName == null) throw new ArgumentNullException(nameof(userName));
        if (firstName == null) throw new ArgumentNullException(nameof(firstName));
        if (lastName == null) throw new ArgumentNullException(nameof(lastName));
        if (email == null) throw new ArgumentNullException(nameof(email));
        if (pub == null) throw new ArgumentNullException(nameof(pub));
        try {
            var profile = Profile.Create(userId).Payload;
            var createdAt = CreatedAtType.Create().Payload!;
            var user = new User(userId, userName, firstName, lastName, email, pub, createdAt, profile);
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