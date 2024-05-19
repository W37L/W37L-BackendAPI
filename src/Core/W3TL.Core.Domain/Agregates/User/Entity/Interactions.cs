using W3TL.Core.Domain.Common.Bases;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.User.Entity;

/// <summary>
/// Represents the interactions of a user, including highlights, reported users, retweeted tweets, followers, following, blocked users, muted users, and liked posts.
/// </summary>
public class Interactions : Entity<UserID>
{
    private Interactions() : base(default!) { }

    /// <summary>
    /// Gets or sets the list of highlighted posts.
    /// </summary>
    public List<PostId> Highlights { get; private set; } = new();

    /// <summary>
    /// Gets or sets the list of reported users.
    /// </summary>
    public List<UserID> ReportedUsers { get; private set; } = new();

    /// <summary>
    /// Gets or sets the list of retweeted tweets.
    /// </summary>
    public List<PostId> RetweetedTweets { get; private set; } = new();

    /// <summary>
    /// Gets or sets the list of followers.
    /// </summary>
    public List<UserID> Followers { get; private set; } = new();

    /// <summary>
    /// Gets or sets the list of users being followed.
    /// </summary>
    public List<UserID> Following { get; private set; } = new();

    /// <summary>
    /// Gets or sets the list of blocked users.
    /// </summary>
    public List<UserID> Blocked { get; private set; } = new();

    /// <summary>
    /// Gets or sets the list of muted users.
    /// </summary>
    public List<UserID> Muted { get; private set; } = new();

    /// <summary>
    /// Gets or sets the list of liked posts.
    /// </summary>
    public List<PostId> Likes { get; private set; } = new();

    /// <summary>
    /// Creates an instance of Interactions for the specified user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>An instance of Interactions.</returns>
    public static Result<Interactions> Create(UserID userId)
    {
        return new Interactions();
    }
}