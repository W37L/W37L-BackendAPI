using ViaEventAssociation.Core.Domain.Common.Bases;

namespace W3TL.Core.Domain.Agregates.User.Entity.Values;

public class Count : ValueObject {
    public int Value { get; private set; }

    private Count(int value) {
        Value = value;
    }

    public static Result<Count> Create(int value) {
        try {
            var validation = Validate(value);
            if (validation.IsFailure)
                return validation.Error;
            return new Count(value);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public static Result<Count> Create(string value) {
        try {
            var validation = Validate(int.Parse(value));
            if (validation.IsFailure)
                return validation.Error;
            return new Count(int.Parse(value));
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    private static Result Validate(int value) {
        var errors = new HashSet<Error>();

        if (value < 0)
            errors.Add(Error.NegativeValue);

        if (errors.Any())
            return Error.CompileErrors(errors);

        return Result.Ok;
    }

    public static Count Zero => new Count(0);


    protected override IEnumerable<object> GetEqualityComponents() {
        throw new NotImplementedException();
    }
}