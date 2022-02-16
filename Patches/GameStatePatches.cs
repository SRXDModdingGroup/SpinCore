using System;
using System.IO;
using HarmonyLib;
using SpinCore.Handlers;
using SpinCore.UI;

namespace SpinCore.Patches
{
    internal static class GameStatePatches
    {
        [HarmonyPatch(typeof(GameStateManager), "GetGameStateForType"), HarmonyPrefix]
        private static bool GameStateManager_GetGameStateForType_Prefix(GameStateManager __instance, GameStateManager.GameState stateType, ref GameState __result)
        {
            foreach (var pair in MenuManager.MenuGroups) {
                var menuGroup = pair.Value;
                
                if ((int) stateType != menuGroup.GameStateValue)
                    continue;

                __result = menuGroup.BaseMenuGroup.gameState;
                    
                return false;
            }
            
            return true;
        }
        
        [HarmonyPatch(typeof(GameStateManager), "ApplyStartupSettings")]
        [HarmonyPostfix]
        private static void GameStateManager_ApplyStartupSettings_Postfix(GameStateManager __instance)
        {
            string fileDirectory = "";
            string[] arguments = Environment.GetCommandLineArgs();
            
            foreach (string arg in arguments)
            {
                switch (arg)
                {
                    case "custom_path":
                        int i = (Array.IndexOf(arguments, arg) + 1);
                        fileDirectory = Path.GetFullPath(arguments[i]);
                        break;
                }
            }
            
            if (fileDirectory.Length == 0)
            {
                fileDirectory = Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "..", "AppData", "LocalLow", "Super Spin Digital", "Spin Rhythm XD", "Custom"));
            }
            
            FilePathHandler.Init(fileDirectory);
        }
    }
}
