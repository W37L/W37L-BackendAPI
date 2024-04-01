using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.User.Entity.Values;

public class BioType : ValueObject {
    public static readonly int MAX_LENGTH = 500;

    private BioType(string? value) {
        Value = value;
    }

    public string? Value { get; }

    public static Result<BioType> Create(string? value) {
        try {
            var validation = Validate(value);
            if (validation.IsFailure)
                return validation.Error;
            return new BioType(value);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    private static Result Validate(string? value) {
        var errors = new HashSet<Error>();

        if (value is null)
            return Error.BlankOrNullString;

        if (value.Length > MAX_LENGTH)
            errors.Add(Error.TooLongBio(MAX_LENGTH));

        if (errors.Any())
            return Error.CompileErrors(errors);

        return Result.Ok;
    }

    protected override IEnumerable<object?> GetEqualityComponents() {
        yield return Value;
    }

    public override string? ToString() {
        return Value;
    }
}