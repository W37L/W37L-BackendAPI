using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.User.Entity.Values;

public class WebsiteType : WebLinkType {
    private WebsiteType(string url) : base(url) { }

    public static Result<WebsiteType> Create(string url) {
        var validation = Validate(url);
        if (validation.IsFailure) {
            return validation.Error;
        }

        return new WebsiteType(url);
    }
}