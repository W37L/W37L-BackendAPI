using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.User.Entity.Values;

public class LocationType : ValueObject {
    public static readonly int MAX_LENGTH = 100;
    public string Value { get; }

    private LocationType(string value) {
        Value = value;
    }

    public static Result<LocationType> Create(string value) {
        try {
            var validation = Validate(value);
            if (validation.IsFailure)
                return validation.Error;
            return new LocationType(value);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    private static Result Validate(string value) {
        var errors = new HashSet<Error>();

        if (value is null)
            return Error.BlankString;

        if (string.IsNullOrEmpty(value))
            errors.Add(Error.BlankString);

        if (value.Length > MAX_LENGTH)
            errors.Add(Error.TooLongLocation(MAX_LENGTH));

        if (errors.Any())
            return Error.CompileErrors(errors);

        return Result.Ok;
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }

    public override string ToString() => Value;
}