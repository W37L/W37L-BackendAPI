using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.User.Entity.Values;

/// <summary>
/// Represents the banner URL of a user.
/// </summary>
public class BannerType : WebLinkType {
    private BannerType(string? url) : base(url) { }

    /// <summary>
    /// Creates an instance of BannerType with the specified URL.
    /// </summary>
    /// <param name="url">The URL of the banner.</param>
    /// <returns>A result indicating success or failure.</returns>
    public static Result<BannerType> Create(string? url) {
        var validation = Validate(url);
        if (validation.IsFailure) {
            return validation.Error;
        }

        return new BannerType(url);
    }
}