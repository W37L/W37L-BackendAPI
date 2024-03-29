using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Common.Bases;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.User.Entity;

public class Profile : Entity<UserID> {
    public AvatarType? Avatar { get; private set; }
    public BannerType? Banner { get; private set; }
    public BioType? Bio { get; private set; }
    public Count Followers { get; private set; }
    public Count Following { get; private set; }
    public LocationType? Location { get; private set; }
    public WebsiteType? Website { get; private set; }

    private Profile(UserID userId) : base(userId) {
        Followers = Count.Zero;
        Following = Count.Zero;
    }

    public static Result<Profile> Create(UserID userId) {
        try {
            var profile = new Profile(userId);
            return profile;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public static Result<Profile> Create(UserID userId, AvatarType avatar, BannerType banner, BioType bio, Count followers, Count following, LocationType location, WebsiteType website) {
        try {
            var profile = new Profile(userId) {
                Avatar = avatar,
                Banner = banner,
                Bio = bio,
                Followers = followers,
                Following = following,
                Location = location,
                Website = website
            };
            return profile;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateAvatar(AvatarType avatar) {
        try {
            Avatar = avatar;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateBanner(BannerType banner) {
        try {
            Banner = banner;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateBio(BioType bio) {
        try {
            Bio = bio;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateFollowers(Count followers) {
        try {
            Followers = followers;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateFollowing(Count following) {
        try {
            Following = following;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateLocation(LocationType location) {
        try {
            Location = location;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result UpdateWebsite(WebsiteType website) {
        try {
            Website = website;
            return Result.Ok;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

}