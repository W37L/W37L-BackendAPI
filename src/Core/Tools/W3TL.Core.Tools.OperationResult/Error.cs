
public class Error {
    private Error(string message) {
        Message = message;
        Next = null;
    }

    public string Message { get; }
    public Error Next { get; private set; }

    public static Error ExampleError => new("An example error occurred.");
    public static Error Unknown => new("An unknown error occurred.");


    // Method to convert Exception to a generic Error
    public static Error FromException(Exception exception) {
        return new Error(exception.Message);
    }

    private void Append(Error error) {
        if (Next is null)
            Next = error;
        else
            Next.Append(error);
    }

    public static Error Add(HashSet<Error> errors) {
        // Create a new error with the first error in the chain
        var error = errors.First();
        // Add the rest of the errors to the chain
        foreach (var e in errors.Skip(1)) error.Append(e);

        return error;
    }

    public IEnumerable<Error> GetAllErrors() {
        var errors = new List<Error> {this};
        var current = Next;
        while (current is not null) {
            errors.Add(current);
            current = current.Next;
        }

        return errors;
    }

    public override string ToString() {
        // If there are multiple errors, return a string with all of them
        if (Next != null) return $"{Message}\n{Next}";

        return Message;
    }

    // Overriding the equality methods to compare value objects
    public override bool Equals(object obj) {
        if (obj is null || GetType() != obj.GetType())
            return false;

        if (Message != ((Error) obj).Message)
            return false;

        return true;
    }

    public override int GetHashCode() {
        return Message.GetHashCode();
    }
}