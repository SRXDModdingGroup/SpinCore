using HarmonyLib;

namespace SpinCore.Patches
{
    internal static class MenuSelectWheelPatches
    {
        private static int previousWillLandAtIndex;
        
        [HarmonyPatch(typeof(GenericWheelInput), "Update")]
        [HarmonyPostfix]
        private static void GenericWheelInput_Update_PostFix(GenericWheelInput __instance) {
            if (previousWillLandAtIndex == __instance.WillLandAtIndex)
                return;
            
            previousWillLandAtIndex = __instance.WillLandAtIndex;
        }
    }
}
