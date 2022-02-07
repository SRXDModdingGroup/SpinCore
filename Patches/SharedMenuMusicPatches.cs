using HarmonyLib;
using SpinCore.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpinCore.Patches
{
    class SharedMenuMusicPatches
    {
        [HarmonyPatch(typeof(SharedMenuMusic), "Start"), HarmonyPostfix]
        private static void SharedMenuMusic_Start(SharedMenuMusic __instance)
        {
            InstanceHandler.SharedMenuMusicInstance = __instance;
        }
    }
}
