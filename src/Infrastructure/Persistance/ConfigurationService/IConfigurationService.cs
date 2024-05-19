using Microsoft.Extensions.Configuration;

namespace Persistance.ConfigurationService;

/// <summary>
/// Represents a service for accessing configuration values.
/// </summary>
public interface IConfigurationService {
    /// <summary>
    /// Gets the value of the configuration setting associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the configuration setting.</param>
    /// <returns>The value of the configuration setting, or null if the key is not found.</returns>
    string GetConfigurationValue(string key);
}

/// <summary>
/// Provides an implementation of the IConfigurationService interface using an IConfiguration instance.
/// </summary>
public class ConfigurationService : IConfigurationService {
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the ConfigurationService class with the specified IConfiguration instance.
    /// </summary>
    /// <param name="configuration">The IConfiguration instance to use for accessing configuration settings.</param>
    public ConfigurationService(IConfiguration configuration) {
        _configuration = configuration;
    }

    /// <summary>
    /// Gets the value of the configuration setting associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the configuration setting.</param>
    /// <returns>The value of the configuration setting, or null if the key is not found.</returns>
    public string GetConfigurationValue(string key) {
        return _configuration[key];
    }
}