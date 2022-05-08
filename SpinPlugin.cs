using BepInEx;
using SMU.Utilities;
using SpinCore.UI;

namespace SpinCore; 

/// <summary>
/// The base class for all plugins that utilize SpinCore
/// </summary>
public abstract class SpinPlugin : BaseUnityPlugin {
    /// <summary>
    /// Called when first initializing the plugin
    /// </summary>
    /// <remarks>Remember to call base.Awake when overriding this function</remarks>
    protected virtual void Awake() => MenuManager.RegisterSpinPlugin(this);

    /// <summary>
    /// Creates a new config option and a bindable that carries its value
    /// </summary>
    /// <param name="name">The name of the option</param>
    /// <param name="defaultValue">The default value of the option</param>
    /// <typeparam name="T">The type of the option. Must be a type supported by BepInEx's config system</typeparam>
    /// <returns>A bindable that carries the config's value</returns>
    protected Bindable<T> AddBindableConfig<T>(string name, T defaultValue) {
        var configEntry = Config.Bind("Config", name, defaultValue);
        var bindable = new Bindable<T>(configEntry.Value);
            
        bindable.Bind(value => configEntry.Value = value);

        return bindable;
    }

    /// <summary>
    /// Creates a new options tab in the Mod Options menu
    /// </summary>
    /// <param name="name">The name of the tab</param>
    /// <returns></returns>
    protected CustomSpinTab CreateOptionsTab(string name) => MenuManager.ModOptionsGroup.RootMenu.CreateTab(name);

    /// <summary>
    /// Called by SpinCore when initializing menus for each plugin. Use this to create the options tab and any other menus for this plugin
    /// </summary>
    protected internal virtual void CreateMenus() { }

    /// <summary>
    /// Called after all mod menus have been initialized by SpinCore. Use this for any initialization that depends on other menus
    /// </summary>
    protected internal virtual void LateInit() { }
}