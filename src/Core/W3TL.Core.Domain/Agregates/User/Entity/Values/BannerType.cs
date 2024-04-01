using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.User.Entity.Values;

public class BannerType : WebLinkType {
    private BannerType(string? url) : base(url) { }

    public static Result<BannerType> Create(string? url) {
        var validation = Validate(url);
        if (validation.IsFailure) {
            return validation.Error;
        }

        return new BannerType(url);
    }
}