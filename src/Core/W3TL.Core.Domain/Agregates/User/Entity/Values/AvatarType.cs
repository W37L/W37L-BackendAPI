using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.User.Entity.Values;

/// <summary>
/// Represents the avatar URL of a user.
/// </summary>
public class AvatarType : WebLinkType {
    private AvatarType(string? url) : base(url) { }

    /// <summary>
    /// Creates an instance of AvatarType with the specified URL.
    /// </summary>
    /// <param name="url">The URL of the avatar.</param>
    /// <returns>A result indicating success or failure.</returns>
    public static Result<AvatarType> Create(string? url) {
        var validation = Validate(url);
        if (validation.IsFailure) {
            return validation.Error;
        }

        return new AvatarType(url);
    }
}