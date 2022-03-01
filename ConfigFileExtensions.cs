using BepInEx.Configuration;
using SMU.Utilities;

namespace SpinCore {
    public static class ConfigFileExtensions {
        public static Bindable<T> Bind<T>(this ConfigFile config, string name, T defaultValue = default) {
            var configEntry = config.Bind("Config", name, defaultValue);
            var bindable = new Bindable<T>(configEntry.Value);
            
            bindable.Bind(value => configEntry.Value = value);

            return bindable;
        }
    }
}