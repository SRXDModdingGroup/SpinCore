using HarmonyLib;
using SpinCore.UI;

namespace SpinCore.Patches; 

internal static class GameStatePatches {
    [HarmonyPatch(typeof(GameStateManager), "GetGameStateForType"), HarmonyPrefix]
    private static bool GameStateManager_GetGameStateForType_Prefix(GameStateManager __instance, GameStateManager.GameState stateType, ref GameState __result) {
        foreach (var pair in MenuManager.MenuGroups) {
            var menuGroup = pair.Value;
            
            if ((int) stateType != menuGroup.GameStateValue)
                continue;

            __result = menuGroup.BaseMenuGroup.gameState;
            
            return false;
        }
            
        return true;
    }
}