using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.Post.Values;

public class Signature : ValueObject {
    public string Value { get; private set; }

    private Signature(string value) {
        Value = value;
    }

    public static Result<Signature> Create(string value) {
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

    private static Result Validate(string value) {
        var errors = new HashSet<Error>();

        if (value == null)
            return Error.BlankString;

        //TODO: Add validation for signature

        if (string.IsNullOrWhiteSpace(value))
            errors.Add(Error.BlankString);

        if (errors.Any())
            return Error.CompileErrors(errors);

        return Result.Ok;
    }


    public static implicit operator string(Signature signature) => signature.Value;

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}