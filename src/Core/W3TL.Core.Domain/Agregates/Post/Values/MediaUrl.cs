using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.Post.Values;

/// <summary>
/// Creates a new instance of the <see cref="MediaUrl"/> class with the specified URL.
/// </summary>
/// <param name="url">The URL to create the media URL from.</param>
/// <returns>A result indicating success or failure with the created media URL.</returns>
public class MediaUrl : WebLinkType {
    private MediaUrl(string? url) : base(url) { }

    public static Result<MediaUrl> Create(string? url) {
        var validation = Validate(url);
        if (validation.IsFailure) {
            return validation.Error;
        }

        return new MediaUrl(url);
    }
}