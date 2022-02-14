using HarmonyLib;
using SpinCore.UI;

namespace SpinCore.Patches
{
    internal static class MainMenuPatches {
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
        private static void XDMainMenu_OpenMenu_Prefix(XDMainMenu __instance) => MenuManager.Initialize(__instance);
    }
}
