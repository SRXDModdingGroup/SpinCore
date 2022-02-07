using HarmonyLib;
using SpinCore.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpinCore.Patches
{
    class MenuSelectWheelPatches
    {

        static int prevwillLandAtIndex = 0;
        [HarmonyPatch(typeof(GenericWheelInput), "Update")]
        [HarmonyPostfix]
        private static void GenericWheelInputUpdate_PostFix(GenericWheelInput __instance)
        {
            if (prevwillLandAtIndex != __instance.WillLandAtIndex)
            {
                prevwillLandAtIndex = __instance.WillLandAtIndex;
                SMU.Events.EventHelper.InvokeAll(InstanceHandler.OnSelectWheelChangeValue);
            }
        }
    }
}
