using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Common.Values;

/**
* Represents an abstract base class for URL value objects, providing foundational
* validation and normalization of URLs for derived types.
*/
public abstract class WebLinkType : ValueObject {
    protected WebLinkType(string? url) {
        Validate(url).OnFailure(error => throw new Exception(error.Message));
        Url = url;
    }

    public string? Url { get; }

    protected static Result Validate(string? url) {
        if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) &&
            url.StartsWith("www.", StringComparison.OrdinalIgnoreCase)) {
            url = "https://" + url;
        }

        if (!Regex.IsMatch(url,
                @"^(https?:\/\/)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)"))
            return Error.InvalidUrl;

        return Result.Ok;
    }


    /**
    * Provides the basis for equality comparisons between instances of derived types.
    *
    * @return An enumerable of objects used in equality comparison, based on URL.
    */
    protected override IEnumerable<object?> GetEqualityComponents() {
        yield return Url;
    }
}