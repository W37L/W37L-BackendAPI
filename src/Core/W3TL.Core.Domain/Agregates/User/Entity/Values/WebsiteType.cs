using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.User.Entity.Values;

/// <summary>
/// Represents a website URL associated with a user.
/// </summary>
public class WebsiteType : WebLinkType {
    private WebsiteType(string? url) : base(url) { }

    /// <summary>
    /// Creates an instance of WebsiteType with the specified URL.
    /// </summary>
    /// <param name="url">The URL of the website.</param>
    /// <returns>A result indicating success or failure.</returns>
    public static Result<WebsiteType> Create(string? url) {
        var validation = Validate(url);
        if (validation.IsFailure) {
            return validation.Error;
        }

        return new WebsiteType(url);
    }
}