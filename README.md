# SpinCore
SpinCore is a utility mod for Spin Rhythm XD that enables modders to quickly add new menus and UI elements, interact with custom chart data, enable / disable score submission, and more.

### Getting Started

To utilize SpinCore in a new mod, add SpinCore.dll as a dependency to your C# Project and then have your main plugin class derive from SpinPlugin instead of BaseUnityPlugin.
Deriving from SpinPlugin gives access to three overridable initialization functions:

- Awake: Called the moment the plugin is created. Use this for BepInEx config initialization or any other initialializtion that does not depend on SpinCore, as SpinCore might not be initialized yet. Remember to call base.Awake if you override this function.
- Init: Called after SpinCore and the mod options menu are initialized. Create new menus and options menu tabs within this function.
- LateInit: Called after Init has been completed for all plugins.

### Creating an options menu

To create a new options menu tab for your plugin:

- Create bindable config entries for each option you want to add. You can do this by calling Config.CreateBindable within Awake and assigning the result to a static property.
- Create a new options menu tab by calling MenuManager.CreateOptionsTab within Init, and then get a reference to its UIRoot.
- Use the functions provided in SpinUI to create new UI elements and attach them to this root.
- Call the Bind extension method on any UI element to link it with a bindable property.

Example:

```cs
public class Plugin : SpinPlugin {
    public static Bindable<bool> EnableMyPlugin { get; private set; }
    
    protected override void Awake() {
        base.Awake();
        EnableMyPlugin = Config.CreateBindable("EnableMyPlugin", true);
    }

    protected override void Init() {
        var root = MenuManager.CreateOptionsTab("My Plugins").UIRoot;

        SpinUI.CreateToggle("Enable My Plugin", root).Bind(EnableMyPlugin);
    }
}
```
