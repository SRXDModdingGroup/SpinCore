using HarmonyLib;
using SpinCore.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpinCore.Patches
{
    class ScoreSubmissionPatches
    {
        [HarmonyPatch(typeof(XDLevelCompleteMenu), "ProcessGeneralSongComplete"), HarmonyPrefix]
        private static bool XDLevelCompleteMenu_ProcessGeneralSongComplete_Prefix() => ScoreSubmissionHandler.IsScoreSubmissionEnabled;

    }
}
