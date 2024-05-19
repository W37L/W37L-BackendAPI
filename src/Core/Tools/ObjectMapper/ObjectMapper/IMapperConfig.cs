namespace ObjectMapper;

// Interface defining configuration for mapping between specific types
public interface IMappingConfig<TInput, TOutput>
    where TOutput : class
    where TInput : class {
    /// <summary>
    ///     Maps an object of type TInput to an object of type TOutput.
    /// </summary>
    /// <param name="input">The input object to map.</param>
    /// <returns>The mapped object of type TOutput.</returns>
    TOutput Map(TInput input);
}