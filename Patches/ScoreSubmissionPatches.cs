using HarmonyLib;
using SpinCore.Handlers;

namespace SpinCore.Patches
{
    internal static class ScoreSubmissionPatches
    {
        [HarmonyPatch(typeof(XDLevelCompleteMenu), "ProcessGeneralSongComplete"), HarmonyPrefix]
        private static bool XDLevelCompleteMenu_ProcessGeneralSongComplete_Prefix() => ScoreSubmissionHandler.IsScoreSubmissionEnabled;
    }
}
