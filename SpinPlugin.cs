using BepInEx;
using SpinCore.UI;
using UnityEngine;

namespace SpinCore.Behaviours {
    public abstract class SpinPlugin : BaseUnityPlugin {
        public abstract string Name { get; }
        
        protected virtual void Awake() {
            MenuManager.RegisterSpinPlugin(this);
        }

        protected internal abstract void CreateOptionsMenu(Transform root);

        protected internal abstract void LateInit();
    }
}