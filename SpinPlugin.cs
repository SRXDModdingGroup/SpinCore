using BepInEx;
using SpinCore.UI;
using UnityEngine;

namespace SpinCore.Behaviours {
    public abstract class SpinPlugin : BaseUnityPlugin {
        public virtual string Name => Info.Metadata.Name;

        protected virtual void Awake() => MenuManager.RegisterSpinPlugin(this);

        protected Transform CreateOptionsTab(string name) => MenuManager.ModOptionsGroup.RootMenu.CreateTab(name).transform;

        protected internal virtual void CreateMenus() { }

        protected internal virtual void LateInit() { }
    }
}