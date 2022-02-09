using HarmonyLib;
using SimpleJSON;
using SMU.Reflection;
using SMU.Utilities;
using SpinCore.Behaviours;
using SpinCore.Handlers;
using SpinCore.Handlers.UI;
using SpinCore.Patches;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SpinCore
{
    public class MainMenuPatches
    {


        public static void CreateMenuObjects(Transform mainMenuContainer)
        {


            foreach (CustomSpinMenu menu in CustomSpinMenuHandler.MenuList)
            {

                //int gameStateint = gameStateDictionaryNameLookup.GetValueSafe(menu.menuID);

                GameState currentGameState = GameStatePatches.MenuIDToGameState.GetValueSafe(menu.MenuName);
                menu.GameStateVal = GameStatePatches.CurrentDictionaryValue;
                GameStatePatches.CurrentDictionaryValue++;

                menu.SpinMenuGroupObject = GameObject.Instantiate<UnityEngine.GameObject>(UICreationHandler.ExampleMenuGroup, mainMenuContainer);
                menu.SpinMenuGroupObject.SetActive(false);
                SpinMenuGroup newSpinMenuGroup = menu.SpinMenuGroupObject.GetComponent<SpinMenuGroup>();
                menu.SpinMenuGroupObject.name = menu.SpinMenuGroupName;
                newSpinMenuGroup.name = menu.SpinMenuGroupName;
                newSpinMenuGroup.menuType = (GameStateManager.GameState)(menu.GameStateVal);
                newSpinMenuGroup.SetProperty<SpinMenuGroup, GameState>("gameState", currentGameState);
                newSpinMenuGroup.gameState.menuGroup = newSpinMenuGroup;
                GameObject newModMenu = menu.SpinMenuGroupObject.transform.Find("ExampleMenu").gameObject;
                newModMenu.name = $"XD{menu.MenuName}Menu";
                XDOptionsMenu oldXDOptionsMenu = newModMenu.GetComponent<XDOptionsMenu>();
                XDModdedMenu newSpinMenu = newModMenu.AddComponent<XDModdedMenu>();


                newSpinMenuGroup.menus[0] = newSpinMenu;
                newSpinMenu.menuGroup = newSpinMenuGroup;
                newSpinMenu.subMenus = oldXDOptionsMenu.subMenus;
                newSpinMenu.DesiredInAmount = oldXDOptionsMenu.DesiredInAmount;
                newSpinMenu.InAmount = oldXDOptionsMenu.InAmount;
                newSpinMenu.contentParent = oldXDOptionsMenu.contentParent;
                newSpinMenu.buttonPrefab = oldXDOptionsMenu.buttonPrefab;
                newSpinMenu.isSubMenu = oldXDOptionsMenu.isSubMenu;
                newSpinMenu.hideParentWhenOpen = oldXDOptionsMenu.hideParentWhenOpen;
                newSpinMenu.selectDefaultObjectWhenOpened = oldXDOptionsMenu.selectDefaultObjectWhenOpened;
                newSpinMenu.tag = oldXDOptionsMenu.tag;
                newSpinMenu.menuScrollRect = oldXDOptionsMenu.menuScrollRect;
                newSpinMenu.hideFlags = oldXDOptionsMenu.hideFlags;
                newSpinMenu.useGUILayout = oldXDOptionsMenu.useGUILayout;
                newSpinMenu.supportedNavigationType = oldXDOptionsMenu.supportedNavigationType;
                newSpinMenu.closeWhenClickingOutside = oldXDOptionsMenu.closeWhenClickingOutside;
                newSpinMenu.gameStateToChangeToOnExitPress = oldXDOptionsMenu.gameStateToChangeToOnExitPress;
                newSpinMenu.buttonPrefab = oldXDOptionsMenu.buttonPrefab;
                newSpinMenu.overrideCameraTransform = oldXDOptionsMenu.overrideCameraTransform;
                newSpinMenu.snapToCameraOverrideOnOpen = oldXDOptionsMenu.snapToCameraOverrideOnOpen;
                GameObject.DestroyImmediate(oldXDOptionsMenu);

                newSpinMenu.CurrentCustomSpinMenu = menu;

                Transform container = newModMenu.gameObject.transform.Find("Container");

                container.position -= new Vector3(0.5f, 0f, 0f);

                Transform contentArea = container.Find("ContentArea");
                Transform options = contentArea.Find("TopPanel").Find("Options");
                options.GetComponentInChildren<TranslatedTextMeshPro>().text.SetText(menu.MenuName);
                options.GetComponentInChildren<TranslatedTextMeshPro>().enabled = false;
                options.gameObject.name = menu.MenuName;

                UnityEngine.UI.Button backButton = newModMenu.transform.Find("XDBackButton").GetComponentInChildren<UnityEngine.UI.Button>();
                backButton.onClick.AddListener(delegate
                {
                    newSpinMenu.ExitButtonPressed();
                });
                InstanceHandler.SharedMenuMusicInstance.menusToActiveMusic = InstanceHandler.SharedMenuMusicInstance.menusToActiveMusic.AddToArray(newSpinMenu);


                menu.SpinMenuGroupObject.SetActive(true);
                SMU.Events.EventHelper.InvokeAll(menu.OnMenuCreateAction);

                newSpinMenu.SetContentNavigation(backButton, null);


            }
        }






        /*
        [HarmonyPatch(typeof(Wheel), nameof(Wheel.UpdateWheel)), HarmonyPostfix]
        private static void Wheel_UpdateWheel_Postfix(Wheel __instance)
        {
            if(player2 == null)
            {
                player2 = GameObject.Instantiate(__instance.gameObject, __instance.gameObject.transform.parent);

                player2.name = "Wheel2";
            }
            //__instance.name = "Wheel Player 2";
            //__instance.MakeCpuControlledThisFrame();

        }*/

        /*
        
        [HarmonyPatch(typeof(Track), "PlayTrack"), HarmonyPostfix]
        private static void Track_PlayTrack(Track __instance)
        {

            using (GamePatch.SpinPatchBlock spinPatchBlock = GamePatch.PatchBlock<Track>(__instance))
            {
                if (!spinPatchBlock.ShouldNotRunMethod)
                {
                    if (__instance.PlayHandle.IsLoaded())
                    {

                        PlayableTrackData data = __instance.PlayHandle.Data;
                      
                        __instance.playStates = new PlayState[2];
                        MainCamera.Instance.player2IsTutorial = false;

                        //MainCamera.Instance.backgroundCamera2 = null;
                        //MainCamera.Instance.player2AdditionalCameraData = MainCamera.Instance.trackCameraAdditionalData;
                        //MainCamera.Instance.backgroundCamera = MainCamera.Instance.backgroundCamera2;

                        for (int i = 0; i < __instance.playStates.Length; i++)
                        {
                            __instance.playStates[i] = new PlayState(__instance.PlayHandle);
                            __instance.playStates[i].isRadioMode = __instance.IsCurrentlyPlayingRadioSong;
                            
                        }

                        __instance.EnsureStateBuffersAreSizedCorrectly();
                        for (int j = 0; j < __instance.playStates.Length; j++)
                        {
                            PlayState playState = __instance.playStates[j];
                            playState.playerIndex = j;
                            playState.playerId = j;
                            playState.wheel = __instance.wheels.GetElementOrDefault(j);
                            playState.shieldIsEnabled = false;
                            playState.hasShield = playState.shieldIsEnabled;
                            playState.startedPlayingAtUnscaledTime = global::Time.unscaledTime;
                            playState.clearTrailsUntilUnscaledTime = global::Time.unscaledTime + 1f;
                            playState.modifiers = data.Setup.Modifiers;
                            playState.wheel.NewTrackPlaying(playState);
                            if(j== 1)
                                playState.wheel.wheelState.cpuControlled = true;
                        }
                        for (int k = 0; k < __instance.wheels.Length; k++)
                        {
                            PlayState elementOrDefault = __instance.playStates.GetElementOrDefault(k);
                            __instance.wheels[k].gameObject.SetActive(elementOrDefault != null);
                        }
                        __instance.SetupPlayStates(false);
                    }
                }
            }
        }
        */


        [HarmonyPatch(typeof(XDMainMenu), "OpenMenu"), HarmonyPrefix]
        private static void XDMainMenu_Start(XDMainMenu __instance)
        {
            Transform buttonsContainer = __instance.gameObject.transform.Find("TopContainer").Find("ButtonsContainer");
            //Get the buttons container
            if (buttonsContainer.Find("ModdedXDButton") == null)
            {
                //move all buttons up to fit the new, extra button
                buttonsContainer.position += new Vector3(-0.15f, 0.5f, 0f);

                GameObject ModdedXDButtonObject = GameObject.Instantiate<UnityEngine.GameObject>(buttonsContainer.Find("ArcadeXDButton").gameObject, buttonsContainer);
                ModdedXDButtonObject.name = "ModdedXDButton";
                ModdedXDButtonObject.transform.position -= new Vector3(-1.025f, 3f, 0f);
                //creates the button, and moves it into place

                XDButton ModdedXDButton = ModdedXDButtonObject.GetComponentInChildren<XDButton>();
                Button button = ModdedXDButtonObject.GetComponentInChildren<Button>();
                GameObject.Destroy(ModdedXDButtonObject.GetComponentInChildren<TranslatedTextMeshPro>());
                ModdedXDButtonObject.GetComponentInChildren<CustomTextMeshProUGUI>().text = "Mods";

                //apply the text setter to mouse hover
                ModdedXDButton.onSelect.AddListener(delegate {
                    foreach (var component in ModdedXDButton.textsToSet)
                    {
                        component.SetText("Modded Options", true, 0.02f, StockMarketText.CaseType.ToLower, StockMarketText.ScrollType.ScrollNever, 100f);
                    }
                });

                button.onClick.AddListener(delegate
                {
                    SpinCoreMenu.ModMenu.OpenMenu();
                    SpinCoreMenu.ModMenu.GameStateToChangeToOnExitPress = "MainMenu";

                });


                //swap the buttons around
                Transform exitButtonTransform = buttonsContainer.Find("ExitXDButton");
                Vector3 exitPosition = exitButtonTransform.position;
                exitButtonTransform.position = ModdedXDButtonObject.transform.position;
                ModdedXDButtonObject.transform.position = exitPosition;

                Selectable select = buttonsContainer.Find("ExitXDButton").GetComponentInChildren<Button>();
                Navigation navigation = select.navigation;
                navigation.selectOnUp = ModdedXDButtonObject.GetComponentInChildren<Button>();
                select.navigation = navigation;
                //fix the navigation

                Transform mainMenuContainer = __instance.gameObject.transform.parent.parent;
                UICreationHandler.GenerateMenuObjects(mainMenuContainer);
                //setting up the menus on next frame ensures that the objects get deleted correctly
                SMU.Utilities.Dispatcher.QueueForNextFrame(delegate
                {
                    CreateMenuObjects(mainMenuContainer);
                });
                


            }
        }


      
    }
}
