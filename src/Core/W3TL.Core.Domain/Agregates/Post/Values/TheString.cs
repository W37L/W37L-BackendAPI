using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.Post.Values;

public class TheString : ValueObject {
    public static readonly int MAX_LENGTH = 140;
    public string Value { get; private set; }

    private TheString(string value) {
        Value = value;
    }

    public static Result<TheString> Create(string value) {
        try {
            var validation = Validate(value);
            if (validation.IsFailure)
                return validation.Error;
            return new TheString(value);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    private static Result Validate(string value) {
        var errors = new HashSet<Error>();

        if (string.IsNullOrWhiteSpace(value))
            errors.Add(Error.BlankString);

        if (value.Length > MAX_LENGTH)
            errors.Add(Error.TooLongString(MAX_LENGTH));

        if (errors.Any())
            return Error.CompileErrors(errors);

        return Result.Ok;
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}