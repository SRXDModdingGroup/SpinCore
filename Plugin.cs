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
        internal static Plugin Instance;
        
        private void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(this);
                
                return;
            }
            
            Instance = this;

            // Init logs and patches.
            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);

            harmony.PatchAll(typeof(MainMenuPatches));
            harmony.PatchAll(typeof(InstancePatches));
            harmony.PatchAll(typeof(GameStatePatches));
            harmony.PatchAll(typeof(FilePathHandlerPatches));
            harmony.PatchAll(typeof(InstanceHandler));
            harmony.PatchAll(typeof(MenuSelectWheelPatches));

            SpinCoreMenu.InitialiseMenu();
        }

        #region logging
        internal static void LogDebug(string message) => Instance.Log(message, LogLevel.Debug);
        internal static void LogInfo(string message) => Instance.Log(message, LogLevel.Info);
        internal static void LogWarning(string message) => Instance.Log(message, LogLevel.Warning);
        internal static void LogError(string message) => Instance.Log(message, LogLevel.Error);
        private void Log(string message, LogLevel logLevel) => Logger.Log(logLevel, message);
        #endregion
    }
}

