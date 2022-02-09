using HarmonyLib;
using SpinCore.Handlers.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpinCore.Patches
{
    public class GameStatePatches
    {

        public static Dictionary<int, CustomSpinMenu> SpinMenuLookup = new Dictionary<int, CustomSpinMenu>();
        public static int CurrentDictionaryValue = 100;
        public static Dictionary<string, GameState> MenuIDToGameState = new Dictionary<string, GameState>();

        [HarmonyPatch(typeof(GameStateManager), "GetGameStateForType"), HarmonyPrefix]
        private static bool GameStateManager_GetGameStateForType_Prefix(GameStateManager __instance, GameStateManager.GameState stateType, GameState result)
        {
            foreach (CustomSpinMenu menu in CustomSpinMenuHandler.MenuList)
            {
                if ((int)stateType == menu.GameStateVal)
                {
                    result = menu.GameState;
                    return false;
                }
            }
            return true;

        }

        [HarmonyPatch(typeof(GameStateManager), "OpenMenuExclusively"), HarmonyPrefix]
        private static bool GameStateManager_OpenMenuExclusively_Prefix(GameStateManager instance, GameStateManager.GameState state, bool __result)
        {

            foreach (CustomSpinMenu menu in CustomSpinMenuHandler.MenuList)
            {
                if ((int)state == menu.GameStateVal)
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
            Transform worldMenuTransform = __instance.gameObject.transform.Find("WorldMenu");
            if (worldMenuTransform.name == "WorldMenu")
            {
                foreach (CustomSpinMenu menu in CustomSpinMenuHandler.MenuList)
                {
                    string menuID = menu.MenuName;
                    if (worldMenuTransform.Find(menuID) == null)
                    {
                        Transform options = worldMenuTransform.Find("Options");
                        GameObject newGameStateObject = GameObject.Instantiate(options.gameObject, worldMenuTransform);
                        newGameStateObject.name = menuID;
                        //Plugin.LogInfo(menuID);
                    }
                }
            }

            List<GameState> list = new List<GameState>();
            for (int i = 0; i < currentTransform.childCount; i++)
            {
                Transform child = currentTransform.GetChild(i);
                GameState gameState = child.GetComponent<GameState>();
                FieldInfo field = typeof(GameStates).GetField(child.name, BindingFlags.Static | BindingFlags.Public);
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


                foreach (CustomSpinMenu menu in CustomSpinMenuHandler.MenuList)
                {
                    try
                    {
                        if (menu.MenuName == gameState.name && !MenuIDToGameState.ContainsKey(gameState.name))
                        {
                            MenuIDToGameState.Add(menu.MenuName, gameState);
                        }
                    }
                    catch { }

                }

                list.Add(gameState);
                __instance.SetupChildren(gameState, child);
            }


            currentState.childStates = list.ToArray();
            return false;
        }

    }
}
