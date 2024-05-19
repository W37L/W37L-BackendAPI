namespace W3TL.Core.Application.CommandDispatching.Commands;

/// <summary>
/// Represents the base class for all commands.
/// </summary>
public abstract class Command { }

/// <summary>
/// Represents a generic command with an identifier.
/// </summary>
/// <typeparam name="TId">The type of the identifier.</typeparam>
public abstract class Command<TId>(TId id) : Command {
    public TId Id { get; } = id;
}