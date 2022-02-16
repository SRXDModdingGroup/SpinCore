using BepInEx;
using SpinCore.UI;
using UnityEngine;

namespace SpinCore.Behaviours {
    public abstract class SpinPlugin : BaseUnityPlugin {
        public virtual string Name => Info.Metadata.Name;

        protected virtual void Awake() => MenuManager.RegisterSpinPlugin(this);

        protected CustomSpinTab CreateOptionsTab(string name) => MenuManager.ModOptionsGroup.RootMenu.CreateTab(name);

        protected internal virtual void CreateMenus() { }

        protected internal virtual void LateInit() { }
    }
}