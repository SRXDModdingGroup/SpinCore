using HarmonyLib;
using SMU.Utilities;
using SpinCore.Handlers;
using SpinCore.UI;
using SpinCore.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace SpinCore.Patches; 

internal static class MenuPatches {
    private static void CreateModsUI(XDLevelSelectMenuBase levelSelectMenu, string fromState)
    {
        var customFolderButton = levelSelectMenu.sortButton.gameObject;
        var buttonParent = customFolderButton.transform.parent;
        var backingTransform = buttonParent.parent;
        var modsButton = Object.Instantiate(buttonParent.gameObject, backingTransform);
            
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
            
        SpinUI.CreateButton("Open Mods Menu", contextMenu.transform, onClick: () => {
            contextMenu.CloseMenu();
            MenuManager.OpenModOptions(fromState);
        });
    }
        
    [HarmonyPatch(typeof(XDMainMenu), "OpenMenu"), HarmonyPrefix]
    private static void XDMainMenu_OpenMenu_Prefix(XDMainMenu __instance) => MenuManager.Initialize(__instance);

    [HarmonyPatch(typeof(XDLevelSelectMenu), "Start")]
    [HarmonyPostfix]
    private static void XDLevelSelectMenu_Start_Postfix(XDLevelSelectMenu __instance) => CreateModsUI(__instance, "LevelSelect");

    [HarmonyPatch(typeof(XDCustomLevelSelectMenu), "Start")]
    [HarmonyPostfix]
    private static void XDCustomLevelSelectMenu_Start_Postfix(XDCustomLevelSelectMenu __instance)
    {
        CreateModsUI(__instance, "CustomLevelSelect");
        InstanceHandler.XDCustomLevelSelectMenu = __instance;
    }

    [HarmonyPatch(typeof(XDLevelCompleteMenu), "ProcessGeneralSongComplete"), HarmonyPrefix]
    private static bool XDLevelCompleteMenu_ProcessGeneralSongComplete_Prefix() => ScoreSubmissionUtility.IsScoreSubmissionEnabled;
        
    [HarmonyPatch(typeof(SharedMenuMusic), "Start"), HarmonyPostfix]
    private static void SharedMenuMusic_Start(SharedMenuMusic __instance) => InstanceHandler.SharedMenuMusic = __instance;
}