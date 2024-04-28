using W3TL.Core.Domain.Common.Bases;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.User.Entity;

public class Interactions : Entity<UserID> {
    private Interactions(UserID userId) : base(userId) {
        Highlights = new List<PostId>();
        ReportedUsers = new List<UserID>();
        RetweetedTweets = new List<PostId>();
        Followers = new List<UserID>();
        Following = new List<UserID>();
        Blocked = new List<UserID>();
        Muted = new List<UserID>();
        Likes = new List<PostId>();
    }

    public Interactions() : base(default!) {
        Highlights = new List<PostId>();
        ReportedUsers = new List<UserID>();
        RetweetedTweets = new List<PostId>();
        Followers = new List<UserID>();
        Following = new List<UserID>();
        Blocked = new List<UserID>();
        Muted = new List<UserID>();
        Likes = new List<PostId>();
    }


    public List<PostId> Highlights { get; private set; }
    public List<UserID> ReportedUsers { get; private set; }
    public List<PostId> RetweetedTweets { get; private set; }
    public List<UserID> Followers { get; private set; }
    public List<UserID> Following { get; private set; }
    public List<UserID> Blocked { get; private set; }
    public List<UserID> Muted { get; private set; }
    public List<PostId> Posts { get; }
    public List<PostId> Likes { get; private set; }

    public static Result<Interactions> Create(UserID userId) {
        return new Interactions();
    }
}