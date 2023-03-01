using HarmonyLib;
using SpinCore.UI;

namespace SpinCore.Patches; 

internal static class GameStatePatches {
    // [HarmonyPatch(typeof(GameStateManager), "GetGameStateForType"), HarmonyPrefix]
    // private static bool GameStateManager_GetGameStateForType_Prefix(GameStateManager.GameState stateType, ref GameState __result) {
    //     if (!MenuManager.TryGetGameStateForStateType(stateType, out var gameState))
    //         return true;
    //     
    //     __result = gameState;
    //
    //     return false;
    // }
}