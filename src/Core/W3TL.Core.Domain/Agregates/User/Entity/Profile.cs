using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Common.Bases;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.User.Entity;

public class Profile : Entity<UserID> {
    private Profile(UserID userId) : base(userId) {
        Followers = Count.Zero;
        Following = Count.Zero;
    }

    public AvatarType? Avatar { get; private set; }
    public BannerType? Banner { get; private set; }
    public BioType? Bio { get; private set; }
    public Count Followers { get; internal set; }
    public Count Following { get; internal set; }
    public LocationType? Location { get; private set; }
    public WebsiteType? Website { get; private set; }

    public static Result<Profile> Create(UserID userId) {
        var profile = new Profile(userId);
        return profile;
    }


    public Result UpdateAvatar(AvatarType avatar) {
        Avatar = avatar;
        return Result.Ok;
    }

    public Result UpdateBanner(BannerType banner) {
        Banner = banner;
        return Result.Ok;
    }

    public Result UpdateBio(BioType bio) {
        Bio = bio;
        return Result.Ok;
    }

    public Result UpdateWebsite(WebsiteType website) {
        Website = website;
        return Result.Ok;
    }

    public Result UpdateLocation(LocationType location) {
        Location = location;
        return Result.Ok;
    }


    public Result IncrementFollowers() {
        return Followers.Increment();
    }

    public Result DecrementFollowers() {
        return Followers.Decrement();
    }

    public Result IncrementFollowing() {
        return Following.Increment();
    }

    public Result DecrementFollowing() {
        return Following.Decrement();
    }
}