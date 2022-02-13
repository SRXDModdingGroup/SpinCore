using HarmonyLib;
using SpinCore.Handlers;
using SpinCore.Handlers.UI;

namespace SpinCore.Patches
{
    internal static class InstancePatches
    {
        [HarmonyPatch(typeof(SharedMenuMusic), "Start"), HarmonyPostfix]
        private static void SharedMenuMusic_Start(SharedMenuMusic __instance)
        {
            InstanceHandler.SharedMenuMusicInstance = __instance;
        }

        [HarmonyPatch(typeof(XDLevelSelectMenu), "Start")]
        [HarmonyPostfix]
        private static void XDLevelSelectMenu_Start_Postfix(XDLevelSelectMenu __instance)
        {
            InstanceHandler.XDLevelSelectMenuInstance = __instance;
            var button = ModsButtonCreationHandler.CreateModsButton(__instance);

            SpinCoreMenu.OfficialLevelsOptionsContextMenu = new CustomContextMenu("Quick Options", __instance);

            new CustomButton("Open Mods Menu", SpinCoreMenu.OfficialLevelsOptionsContextMenu, delegate {
                SpinCoreMenu.OfficialLevelsOptionsContextMenu.CloseMenu();
                SpinCoreMenu.ModMenu.Open();
                SpinCoreMenu.ModMenu.GameStateToChangeToOnExitPress = "LevelSelect";
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
            var button = ModsButtonCreationHandler.CreateModsButton(__instance);

            SpinCoreMenu.CustomLevelsOptionsContextMenu = new CustomContextMenu("Quick Options", __instance);

            new CustomButton("Open Mods Menu", SpinCoreMenu.CustomLevelsOptionsContextMenu, delegate {
                SpinCoreMenu.CustomLevelsOptionsContextMenu.CloseMenu();
                SpinCoreMenu.ModMenu.Open();
                SpinCoreMenu.ModMenu.GameStateToChangeToOnExitPress = "CustomLevelSelect";
            });

            button.onClick.AddListener(delegate
            {
                SpinCoreMenu.CustomLevelsOptionsContextMenu.OpenMenu();
            });


            SMU.Events.EventHelper.InvokeAll(InstanceHandler.OnCustomLevelsOpen);
        }


    }
}
