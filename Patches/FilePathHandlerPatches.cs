using HarmonyLib;
using SpinCore.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpinCore.Patches
{
    class FilePathHandlerPatches
    {

        [HarmonyPatch(typeof(GameStateManager), "ApplyStartupSettings")]
        [HarmonyPostfix]
        private static void ChangeState_Postfix(GameStateManager __instance)
        {
            GameObject obj = new GameObject();
            obj.AddComponent<SMU.Utilities.Dispatcher>();

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
            FilePathHandler.HandleConfig(fileDirectory);
        }



    }
}
