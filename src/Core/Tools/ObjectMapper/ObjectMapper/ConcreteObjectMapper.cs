using System.Text.Json;
using ObjectMapper;

public class ConcreteObjectMapper : IMapper {
    private readonly IServiceProvider _serviceProvider;

    public ConcreteObjectMapper(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public TOutput Map<TOutput>(object input) where TOutput : class {
        Type type = typeof(IMappingConfig<,>).MakeGenericType(input.GetType(), typeof(TOutput));
        dynamic mappingConfig = _serviceProvider.GetService(type);

        if (mappingConfig != null)
            return mappingConfig.Map((dynamic)input);

        string toJson = JsonSerializer.Serialize(input);
        return JsonSerializer.Deserialize<TOutput>(toJson);
    }
}