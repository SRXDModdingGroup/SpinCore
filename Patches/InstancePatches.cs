using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpinCore.Handlers;
using UnityEngine;
using SpinCore.Handlers.UI;
using UnityEngine.UI;
using SMU.Utilities;

namespace SpinCore.Patches
{
    class InstancePatches
    {


        [HarmonyPatch(typeof(XDLevelSelectMenu), "Start")]
        [HarmonyPostfix]
        private static void XDLevelSelectMenu_Start_Postfix(XDLevelSelectMenu __instance)
        {
            InstanceHandler.XDLevelSelectMenuInstance = __instance;
            Button button = ModsButtonCreationHandler.CreateModsButton(__instance);

            SpinCoreMenu.OfficialLevelsOptionsContextMenu = new CustomContextMenu("Quick Options", __instance);

            new CustomButton("Open Mods Menu", SpinCoreMenu.OfficialLevelsOptionsContextMenu, delegate {
                SpinCoreMenu.OfficialLevelsOptionsContextMenu.CloseMenu();
                SpinCoreMenu.ModMenu.OpenMenu("LevelSelect");
            });

            button.onClick.AddListener(delegate
            {
                SpinCoreMenu.OfficialLevelsOptionsContextMenu.OpenMenu();
            });


            SMU.Events.EventHelper.InvokeAll(InstanceHandler.OnOfficialLevelsOpen);

        }

        [HarmonyPatch(typeof(XDCustomLevelSelectMenu), "Start")]
        [HarmonyPostfix]
        private static void XDCustomLevelSelectMenu_Start_Postfix(XDCustomLevelSelectMenu __instance)
        {
            InstanceHandler.XDCustomLevelSelectMenuInstance = __instance;

            Button button = ModsButtonCreationHandler.CreateModsButton(__instance);

            SpinCoreMenu.CustomLevelsOptionsContextMenu = new CustomContextMenu("Quick Options", __instance);

            new CustomButton("Open Mods Menu", SpinCoreMenu.CustomLevelsOptionsContextMenu, delegate {
                SpinCoreMenu.CustomLevelsOptionsContextMenu.CloseMenu();
                SpinCoreMenu.ModMenu.OpenMenu("CustomLevelSelect");
            });

            button.onClick.AddListener(delegate
            {
                SpinCoreMenu.CustomLevelsOptionsContextMenu.OpenMenu();
            });


            SMU.Events.EventHelper.InvokeAll(InstanceHandler.OnCustomLevelsOpen);
        }


    }
}
