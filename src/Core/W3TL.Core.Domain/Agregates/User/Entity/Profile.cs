using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Common.Bases;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.User.Entity;

/// <summary>
/// Represents the profile of a user, including avatar, banner, bio, follower count, following count, location, website, and verification status.
/// </summary>
public class Profile : Entity<UserID> {

    private Profile(UserID userId) : base(userId) {
        Followers = Count.Zero;
        Following = Count.Zero;
    }

    /// <summary>
    /// Gets or sets the avatar of the user profile.
    /// </summary>
    public AvatarType? Avatar { get; private set; }
    
    /// <summary>
    /// Gets or sets the banner of the user profile.
    /// </summary>
    public BannerType? Banner { get; private set; }
    
    /// <summary>
    /// Gets or sets the bio of the user profile.
    /// </summary>
    public BioType? Bio { get; private set; }
    
    /// <summary>
    /// Gets or sets the count of followers.
    /// </summary>
    public Count Followers { get; internal set; }
    
    /// <summary>
    /// Gets or sets the count of users being followed.
    /// </summary>
    public Count Following { get; internal set; }
    
    /// <summary>
    /// Gets or sets the location of the user profile.
    /// </summary>
    public LocationType? Location { get; private set; }
    
    /// <summary>
    /// Gets or sets the website of the user profile.
    /// </summary>
    public WebsiteType? Website { get; private set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the user profile is verified.
    /// </summary>
    public bool Verified { get; private set; } = false;

    /// <summary>
    /// Creates a profile for the specified user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>An instance of Profile.</returns>
    public static Result<Profile> Create(UserID userId) {
        var profile = new Profile(userId);
        return profile;
    }

    /// <summary>
    /// Updates the avatar of the user profile.
    /// </summary>
    /// <param name="avatar">The new avatar of the user profile.</param>
    /// <returns>A result indicating success or failure.</returns>
    public Result UpdateAvatar(AvatarType avatar) {
        Avatar = avatar;
        return Result.Ok;
    }

    /// <summary>
    /// Updates the banner of the user profile.
    /// </summary>
    /// <param name="banner">The new banner of the user profile.</param>
    /// <returns>A result indicating success or failure.</returns>
    public Result UpdateBanner(BannerType banner) {
        Banner = banner;
        return Result.Ok;
    }

    /// <summary>
    /// Updates the bio of the user profile.
    /// </summary>
    /// <param name="bio">The new bio of the user profile.</param>
    /// <returns>A result indicating success or failure.</returns>
    public Result UpdateBio(BioType bio) {
        Bio = bio;
        return Result.Ok;
    }

    /// <summary>
    /// Updates the website of the user profile.
    /// </summary>
    /// <param name="website">The new website of the user profile.</param>
    /// <returns>A result indicating success or failure.</returns>
    public Result UpdateWebsite(WebsiteType website) {
        Website = website;
        return Result.Ok;
    }

    /// <summary>
    /// Updates the location of the user profile.
    /// </summary>
    /// <param name="location">The new location of the user profile.</param>
    /// <returns>A result indicating success or failure.</returns>
    public Result UpdateLocation(LocationType location) {
        Location = location;
        return Result.Ok;
    }

    /// <summary>
    /// Increments the count of followers.
    /// </summary>
    /// <returns>A result indicating success or failure.</returns>
    public Result IncrementFollowers() {
        return Followers.Increment();
    }

    /// <summary>
    /// Decrements the count of followers.
    /// </summary>
    /// <returns>A result indicating success or failure.</returns>
    public Result DecrementFollowers() {
        return Followers.Decrement();
    }

    /// <summary>
    /// Increments the count of users being followed.
    /// </summary>
    /// <returns>A result indicating success or failure.</returns>
    public Result IncrementFollowing() {
        return Following.Increment();
    }

    /// <summary>
    /// Decrements the count of users being followed.
    /// </summary>
    /// <returns>A result indicating success or failure.</returns>
    public Result DecrementFollowing() {
        return Following.Decrement();
    }
}