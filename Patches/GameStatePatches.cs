using HarmonyLib;
using SpinCore.Handlers.UI;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SpinCore.Patches
{
    internal static class GameStatePatches
    {

        public static Dictionary<int, CustomSpinMenuGroup> SpinMenuLookup = new Dictionary<int, CustomSpinMenuGroup>();

        internal static Dictionary<string, GameState> MenuIDToGameState { get; } = new Dictionary<string, GameState>();

        [HarmonyPatch(typeof(GameStateManager), "GetGameStateForType"), HarmonyPrefix]
        private static bool GameStateManager_GetGameStateForType_Prefix(GameStateManager __instance, GameStateManager.GameState stateType, ref GameState __result)
        {
            foreach (CustomSpinMenuGroup menu in CustomSpinMenuHandler.MenuList) {
                if ((int) stateType != menu.GameStateValue)
                    continue;
                
                __result = menu.GameState;
                    
                return false;
            }
            
            return true;
        }

        [HarmonyPatch(typeof(GameStateManager), "OpenMenuExclusively"), HarmonyPrefix]
        private static bool GameStateManager_OpenMenuExclusively_Prefix(GameStateManager __instance, GameStateManager.GameState state, bool __result)
        {
            foreach (CustomSpinMenuGroup menu in CustomSpinMenuHandler.MenuList)
            {
                if ((int)state == menu.GameStateValue)
                {
                    if (menu.GameState)
                    {
                        menu.GameState.BecomeActive();
                    }
                    
                    return false;
                }
            }
            
            return true;
        }

        [HarmonyPatch(typeof(RootGameState), "SetupChildren"), HarmonyPrefix]
        private static bool GameStateManager_SetupChildren_Prefix(RootGameState __instance, GameState currentState, Transform currentTransform)
        {
            var worldMenuTransform = __instance.gameObject.transform.Find("WorldMenu");
            
            if (worldMenuTransform.name == "WorldMenu")
            {
                foreach (CustomSpinMenuGroup menu in CustomSpinMenuHandler.MenuList)
                {
                    string menuID = menu.Name;
                    
                    if (worldMenuTransform.Find(menuID) == null)
                    {
                        var options = worldMenuTransform.Find("Options");
                        var newGameStateObject = Object.Instantiate(options.gameObject, worldMenuTransform);
                        
                        newGameStateObject.name = menuID;
                        //Plugin.LogInfo(menuID);
                    }
                }
            }

            var list = new List<GameState>();
            
            for (int i = 0; i < currentTransform.childCount; i++)
            {
                var child = currentTransform.GetChild(i);
                var gameState = child.GetComponent<GameState>();
                var field = typeof(GameStates).GetField(child.name, BindingFlags.Static | BindingFlags.Public);
                
                if (field != null && typeof(GameState).IsAssignableFrom(field.FieldType) && field.GetValue(null) == null && !field.FieldType.IsInstanceOfType(gameState) && !gameState)
                {
                    gameState = (GameState)child.gameObject.AddComponent(field.FieldType);
                }
                
                if (!gameState)
                {
                    gameState = child.gameObject.AddComponent<GameState>();
                }
                
                if (field != null && field.FieldType.IsInstanceOfType(gameState))
                {
                    field.SetValue(null, gameState);
                }
                
                gameState.parentState = currentState;
                gameState.siblingIndex = list.Count;
                
                foreach (CustomSpinMenuGroup menu in CustomSpinMenuHandler.MenuList)
                {
                    if (menu.Name == gameState.name && !MenuIDToGameState.ContainsKey(gameState.name))
                    {
                        MenuIDToGameState.Add(menu.Name, gameState);
                    }
                }

                list.Add(gameState);
                __instance.SetupChildren(gameState, child);
            }
            
            currentState.childStates = list.ToArray();
            return false;
        }

    }
}
