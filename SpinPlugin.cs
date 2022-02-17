using BepInEx;
using SMU.Utilities;
using SpinCore.UI;

namespace SpinCore.Behaviours {
    public abstract class SpinPlugin : BaseUnityPlugin {
        public virtual string Name => Info.Metadata.Name;

        protected virtual void Awake() => MenuManager.RegisterSpinPlugin(this);

        protected Bindable<T> AddBindableConfig<T>(string name, T defaultValue) {
            var configEntry = Config.Bind("Config", name, defaultValue);
            var bindable = new Bindable<T>(configEntry.Value);
            
            bindable.Bind(value => configEntry.Value = value);

            return bindable;
        }

        protected CustomSpinTab CreateOptionsTab(string name) => MenuManager.ModOptionsGroup.RootMenu.CreateTab(name);

        protected internal virtual void CreateMenus() { }

        protected internal virtual void LateInit() { }
    }
}