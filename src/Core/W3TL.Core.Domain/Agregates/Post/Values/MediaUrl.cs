using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.Post.Values;

public class MediaUrl : WebLinkType {
    private MediaUrl(string url) : base(url) { }

    public static Result<WebLinkType> Create(string url) {
        var validation = Validate(url);
        if (validation.IsFailure) {
            return validation.Error;
        }

        return new MediaUrl(url);
    }
}