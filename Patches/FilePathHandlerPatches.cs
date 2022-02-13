using HarmonyLib;
using SpinCore.Handlers;
using System;
using System.IO;

namespace SpinCore.Patches
{
    internal static class FilePathHandlerPatches
    {
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
