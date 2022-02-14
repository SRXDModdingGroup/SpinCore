using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SpinCore.Handlers.UI;
using SpinCore.Handlers;
using SpinCore.Patches;

namespace SpinCore
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    internal class Plugin : BaseUnityPlugin
    {
        private static Plugin instance;
        
        private void Awake() {
            if (instance) {
                DestroyImmediate(this);
                
                return;
            }
            
            instance = this;

            // Init logs and patches.
            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);

            harmony.PatchAll(typeof(MainMenuPatches));
            harmony.PatchAll(typeof(InstancePatches));
            harmony.PatchAll(typeof(GameStatePatches));
            harmony.PatchAll(typeof(FilePathHandlerPatches));
            harmony.PatchAll(typeof(InstanceHandler));
            harmony.PatchAll(typeof(MenuSelectWheelPatches));
        }

        #region logging
        
        internal static void LogDebug(string message) => instance.Log(message, LogLevel.Debug);
        
        internal static void LogInfo(string message) => instance.Log(message, LogLevel.Info);
        
        internal static void LogWarning(string message) => instance.Log(message, LogLevel.Warning);
        
        internal static void LogError(string message) => instance.Log(message, LogLevel.Error);
        
        private void Log(string message, LogLevel logLevel) => Logger.Log(logLevel, message);
        
        #endregion
    }
}

