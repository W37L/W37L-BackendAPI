using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Common.Values;

/**
* Represents an abstract base class for URL value objects, providing foundational
* validation and normalization of URLs for derived types.
*/
public abstract class WebLinkType : ValueObject {
    /**
 * The URL value encapsulated by this instance.
 */
    public string Url { get; }

    /**
 * Constructs a WebLinkType instance with a specified URL.
 * This constructor validates and, if necessary, normalizes the URL (e.g., prepending "https://").
 *
 * @param url The URL to encapsulate.
 * @throws Exception If the URL fails validation.
 */
    protected WebLinkType(string url) {
        Validate(url).OnFailure(error => throw new Exception(error.Message));

        if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) &&
            url.StartsWith("www.", StringComparison.OrdinalIgnoreCase)) {
            url = "https://" + url;
        }

        Url = url;
    }

    /**
 * Validates the provided URL against a series of rules to ensure it is well-formed.
 *
 * @param url The URL to validate.
 * @return A Result object indicating success, or containing errors if the validation fails.
 */
    protected Result Validate(string url) {
        var errors = new HashSet<Error>();

        if (url == null) {
            return Error.BlankString;
        }

        if (string.IsNullOrWhiteSpace(url)) {
            errors.Add(Error.BlankString);
        }

        if (!Regex.IsMatch(url, @"^(https?:\/\/)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)")) {
            errors.Add(Error.InvalidUrl);
        }

        if (errors.Any()) {
            return Error.CompileErrors(errors);
        }

        return Result.Ok;
    }

    /**
 * Provides the basis for equality comparisons between instances of derived types.
 *
 * @return An enumerable of objects used in equality comparison, based on URL.
 */
    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Url;
    }

    /**
 * Creates a new instance of a derived type with the specified URL.
 *
 * @param url The URL to encapsulate.
 * @return A new instance of a derived type with the specified URL.
 */
    public abstract Result<WebLinkType> Create(string url);
}