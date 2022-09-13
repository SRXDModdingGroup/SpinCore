using BepInEx;
using HarmonyLib;
using SpinCore.Patches;

namespace SpinCore; 

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
internal sealed class Plugin : BaseUnityPlugin
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

        harmony.PatchAll(typeof(MenuPatches));
        harmony.PatchAll(typeof(GameStatePatches));
    }

    #region logging
        
    internal static void LogDebug(string message) => instance.Logger.LogDebug(message);
        
    internal static void LogInfo(string message) => instance.Logger.LogMessage(message);
        
    internal static void LogWarning(string message) => instance.Logger.LogWarning(message);
        
    internal static void LogError(string message) => instance.Logger.LogError(message);
        
    #endregion
}