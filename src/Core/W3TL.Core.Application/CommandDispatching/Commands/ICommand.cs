namespace W3TL.Core.Application.CommandDispatching.Commands;

public interface ICommand<TCommand> where TCommand : Command {
    public static abstract int ParametersCount { get; }
    public static abstract Result<TCommand> Create(params object[] args);
}