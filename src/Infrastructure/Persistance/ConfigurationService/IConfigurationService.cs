using Microsoft.Extensions.Configuration;

namespace Persistance.ConfigurationService;

public interface IConfigurationService {
    string GetConfigurationValue(string key);
}

public class ConfigurationService : IConfigurationService {
    private readonly IConfiguration _configuration;

    public ConfigurationService(IConfiguration configuration) {
        _configuration = configuration;
    }

    public string GetConfigurationValue(string key) {
        return _configuration[key];
    }
}