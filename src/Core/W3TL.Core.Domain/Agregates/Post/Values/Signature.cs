using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.Post.Values;

public class Signature : ValueObject {
    private static readonly Regex HexRegex = new("^[a-fA-F0-9]{128}$");
    private static readonly Regex Base64Regex = new("^[A-Za-z0-9+/]{86}==$|^[A-Za-z0-9+/]{87}=$|^[A-Za-z0-9+/]{88}$\n");

    private Signature(string? value) {
        Value = value;
    }

    public string? Value { get; }

    public static Result<Signature?> Create(string? value) {
        try {
            var validation = Validate(value);
            if (validation.IsFailure)
                return validation.Error;
            return new Signature(value);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    private static Result Validate(string? value) {
        var errors = new HashSet<Error>();

        if (value is null)
            return Error.BlankOrNullString;

        if (!(HexRegex.IsMatch(value) || Base64Regex.IsMatch(value)))
            errors.Add(Error.InvalidSignature);

        if (string.IsNullOrWhiteSpace(value))
            errors.Add(Error.BlankOrNullString);

        if (errors.Any())
            return Error.CompileErrors(errors);

        return Result.Ok;
    }


    public static implicit operator string?(Signature signature) {
        return signature.Value;
    }

    protected override IEnumerable<object?> GetEqualityComponents() {
        yield return Value;
    }
}