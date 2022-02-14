using HarmonyLib;
using SMU.Utilities;
using SpinCore.Handlers;
using SpinCore.Handlers.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SpinCore.Patches
{
    internal static class InstancePatches
    {
        private static void CreateModsUI(XDLevelSelectMenuBase levelSelectMenu, string fromState)
        {
            var customFolderButton = levelSelectMenu.sortButton.gameObject;
            var backingTransform = customFolderButton.transform.parent.parent;
            var modsButton = Object.Instantiate(customFolderButton.transform.parent.gameObject, backingTransform);
            
            modsButton.name = "ModsButton";
            
            var buttonTransform = modsButton.transform.Find("SortButtonImg");
            
            buttonTransform.name = "ModsButtonImg";
            buttonTransform.Find("Imgtop").Find("ImagePlay").gameObject.GetComponent<Image>().sprite = ImageHelper.LoadSpriteFromResources("SpinCore.Images.ModsIcon.png");
            
            var textObj = buttonTransform.transform.Find("Imgtop").Find("Text TMP");
            
            Object.Destroy(textObj.GetComponent<TranslatedTextMeshPro>());
            textObj.GetComponent<CustomTextMeshProUGUI>().SetText("Mods");

            var button = buttonTransform.GetComponentInChildren<Button>();
            var contextMenu = levelSelectMenu.GenerateContextMenu();
            
            button.onClick.AddListener(() => contextMenu.OpenMenu());
            
            SpinUI.CreateButton("Open Mods Menu", contextMenu.transform, () => {
                contextMenu.CloseMenu();
                MenuManager.OpenMenuGroup(MenuManager.ModOptionsGroup, fromState);
            });
        }

        [HarmonyPatch(typeof(XDLevelSelectMenu), "Start")]
        [HarmonyPostfix]
        private static void XDLevelSelectMenu_Start_Postfix(XDLevelSelectMenu __instance) => CreateModsUI(__instance, "LevelSelect");

        [HarmonyPatch(typeof(XDCustomLevelSelectMenu), "Start")]
        [HarmonyPostfix]
        private static void XDCustomLevelSelectMenu_Start_Postfix(XDCustomLevelSelectMenu __instance)
        {
            CreateModsUI(__instance, "CustomLevelSelect");
            InstanceHandler.XDCustomLevelSelectMenuInstance = __instance;
        }
    }
}
