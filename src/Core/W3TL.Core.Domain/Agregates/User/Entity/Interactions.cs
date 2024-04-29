using W3TL.Core.Domain.Common.Bases;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.User.Entity;

public class Interactions : Entity<UserID> {
    private Interactions() : base(default!) { }

    public List<PostId> Highlights { get; private set; } = new();
    public List<UserID> ReportedUsers { get; private set; } = new();
    public List<PostId> RetweetedTweets { get; private set; } = new();
    public List<UserID> Followers { get; private set; } = new();
    public List<UserID> Following { get; private set; } = new();
    public List<UserID> Blocked { get; private set; } = new();
    public List<UserID> Muted { get; private set; } = new();
    public List<PostId> Likes { get; private set; } = new();

    public static Result<Interactions> Create(UserID userId) {
        return new Interactions();
    }
}