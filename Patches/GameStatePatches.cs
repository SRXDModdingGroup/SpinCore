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

        public static Dictionary<int, CustomSpinMenu> spinMenuLookup = new Dictionary<int, CustomSpinMenu>();
        public static int currentDictionaryValue = 100;
        public static Dictionary<string, GameState> menuIDToGameState = new Dictionary<string, GameState>();

        [HarmonyPatch(typeof(GameStateManager), "GetGameStateForType"), HarmonyPrefix]
        private static bool GameStateManager_GetGameStateForType_Prefix(GameStateManager __instance, GameStateManager.GameState stateType, GameState __result)
        {
            foreach (CustomSpinMenu menu in CustomSpinMenuHandler.MenuList)
            {
                if ((int)stateType == menu.gameStateVal)
                {
                    __result = menu.gameState;
                    return false;
                }
            }
            return true;

        }

        [HarmonyPatch(typeof(GameStateManager), "OpenMenuExclusively"), HarmonyPrefix]
        private static bool GameStateManager_OpenMenuExclusively_Prefix(GameStateManager __instance, GameStateManager.GameState state, bool __result)
        {

            foreach (CustomSpinMenu menu in CustomSpinMenuHandler.MenuList)
            {
                if ((int)state == menu.gameStateVal)
                {
                    if (menu.gameState)
                    {
                        menu.gameState.BecomeActive();
                    }
                    return false;
                }
            }
            return true;

        }




        [HarmonyPatch(typeof(RootGameState), "SetupChildren"), HarmonyPrefix]
        private static bool GameStateManager_SetupChildren_Prefix(RootGameState __instance, GameState currentState, Transform currentTransform)
        {
            Transform WorldMenuTransform = __instance.gameObject.transform.Find("WorldMenu");
            if (WorldMenuTransform.name == "WorldMenu")
            {
                foreach (CustomSpinMenu menu in CustomSpinMenuHandler.MenuList)
                {
                    string menuID = menu.menuName;
                    if (WorldMenuTransform.Find(menuID) == null)
                    {
                        Transform Options = WorldMenuTransform.Find("Options");
                        GameObject newGameStateObject = GameObject.Instantiate(Options.gameObject, WorldMenuTransform);
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
                        if (menu.menuName == gameState.name && !menuIDToGameState.ContainsKey(gameState.name))
                        {
                            menuIDToGameState.Add(menu.menuName, gameState);
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
