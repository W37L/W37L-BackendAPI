namespace ObjectMapper;

/// <summary>
///   The IMapper interface is responsible for mapping objects of type TInput to objects of type TOutput.
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public interface IMappingConfig<TInput, TOutput> where TOutput : class where TInput : class {
    
    /// <summary>
    ///     Maps an object of type TInput to an object of type TOutput.
    /// </summary>
    /// <param name="input">The input object to map.</param>
    /// <returns>The mapped object of type TOutput.</returns>
    TOutput Map(TInput input);
}