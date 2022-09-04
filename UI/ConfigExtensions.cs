using BepInEx.Configuration;
using SMU.Utilities;

namespace SpinCore.UI; 

/// <summary>
/// Contains extension methods for BepInEx config
/// </summary>
public static class ConfigExtensions {
    /// <summary>
    /// Creates a new config entry and a bindable that carries its value
    /// </summary>
    /// <param name="configFile">The config file to create an entry for</param>
    /// <param name="section">The section name to group the setting under</param>
    /// <param name="key">The name of the setting</param>
    /// <param name="defaultValue">The default value of the setting</param>
    /// <typeparam name="T">The type of the setting. Must be a type supported by BepInEx's config system</typeparam>
    /// <returns>A bindable that carries the config entry's value</returns>
    public static Bindable<T> CreateBindable<T>(this ConfigFile configFile, string section, string key, T defaultValue) {
        var configEntry = configFile.Bind(section, key, defaultValue);
        var bindable = new Bindable<T>(configEntry.Value);
            
        bindable.Bind(value => configEntry.Value = value);

        return bindable;
    }

    /// <summary>
    /// Creates a new config entry and a bindable that carries its value
    /// </summary>
    /// <param name="configFile">The config file to create an entry for</param>
    /// <param name="key">The name of the setting</param>
    /// <param name="defaultValue">The default value of the setting</param>
    /// <typeparam name="T">The type of the setting. Must be a type supported by BepInEx's config system</typeparam>
    /// <returns>A bindable that carries the config entry's value</returns>
    public static Bindable<T> CreateBindable<T>(this ConfigFile configFile, string key, T defaultValue)
        => CreateBindable(configFile, "Config", key, defaultValue);
}