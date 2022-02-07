using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SpinCore.Handlers.UI;
using System.Reflection;
using Steamworks;
using SpinCore.Handlers;
using System;
using System.IO;
using System.Net;
using SimpleJSON;
using static SpinCore.Handlers.UICreationHandler;
using UnityEngine;
using SpinCore.Patches;
using SMU.Utilities;
using System.Collections.Generic;

namespace SpinCore
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    class Plugin : BaseUnityPlugin
    {
        internal static Plugin Instance;


        void Awake()
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

