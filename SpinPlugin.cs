using BepInEx;
using SpinCore.UI;
using UnityEngine;

namespace SpinCore.Behaviours {
    public abstract class SpinPlugin : BaseUnityPlugin {
        public abstract string Name { get; }

        protected internal virtual bool HasOptionsMenu => false;
        
        protected virtual void Awake() {
            MenuManager.RegisterSpinPlugin(this);
        }

        protected internal virtual void CreateOptionsMenu(Transform root) { }

        protected internal virtual void LateInit() { }
    }
}