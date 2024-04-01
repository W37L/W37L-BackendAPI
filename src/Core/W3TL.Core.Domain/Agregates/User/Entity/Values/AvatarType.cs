using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Domain.Agregates.User.Entity.Values;

public class AvatarType : WebLinkType {
    private AvatarType(string? url) : base(url) { }

    public static Result<AvatarType> Create(string? url) {
        var validation = Validate(url);
        if (validation.IsFailure) {
            return validation.Error;
        }

        return new AvatarType(url);
    }
}