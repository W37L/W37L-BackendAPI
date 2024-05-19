using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Common.Values;

///<summary>
/// Represents an abstract base class for URL value objects, providing foundational
/// validation and normalization of URLs for derived types.
///</summary>
public abstract class WebLinkType : ValueObject {
    protected WebLinkType(string? url) {
        Validate(url).OnFailure(error => throw new Exception(error.Message));
        Url = url;
    }

    /// <summary>
    /// Gets the URL associated with the web link.
    /// </summary>
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

    ///<summary>
    /// Provides the basis for equality comparisons between instances of derived types.
    ///</summary>
    /// <returns>An enumerable of objects used in equality comparison, based on URL.</returns>
    protected override IEnumerable<object?> GetEqualityComponents() {
        yield return Url;
    }
}