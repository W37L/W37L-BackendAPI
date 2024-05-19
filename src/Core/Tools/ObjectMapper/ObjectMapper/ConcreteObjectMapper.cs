using System.Text.Json;
using ObjectMapper;

// Concrete implementation of IMapper
public class ConcreteObjectMapper : IMapper {
    private readonly IServiceProvider _serviceProvider;

    public ConcreteObjectMapper(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }
    
    /// <summary>
    /// Maps an object of type `TInput` to an object of type `TOutput`.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output object.</typeparam>
    /// <param name="input">The object to be mapped.</param>
    /// <returns>The mapped object of type `TOutput`.</returns>
    public TOutput Map<TOutput>(object input) where TOutput : class {
        Type type = typeof(IMappingConfig<,>).MakeGenericType(input.GetType(), typeof(TOutput));
        dynamic mappingConfig = _serviceProvider.GetService(type);

        if (mappingConfig != null)
            return mappingConfig.Map((dynamic)input);

        string toJson = JsonSerializer.Serialize(input);

        // Fallback to serializing and deserializing if no specific mapper is registered
        return JsonSerializer.Deserialize<TOutput>(toJson);
    }
}