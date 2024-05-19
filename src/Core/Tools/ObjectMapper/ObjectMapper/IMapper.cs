namespace ObjectMapper;

/// <summary>
///   The IMapper interface is responsible for mapping objects of type TInput to objects of type TOutput.
/// </summary>
public interface IMapper {
    
    /// <summary>
    ///     Maps an input object to an output object of type TOutput.
    /// </summary>
    /// <param name="input">The input object to map.</param>
    /// <typeparam name="TOutput">The type of the output object.</typeparam>
    /// <returns>The mapped object of type TOutput.</returns>
    TOutput Map<TOutput>(object input) where TOutput : class;
}