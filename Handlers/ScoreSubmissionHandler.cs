using System.Collections.Generic;


namespace SpinCore.Handlers
{
    public static class ScoreSubmissionHandler
    {
        public static bool IsScoreSubmissionEnabled => scoreSubmissionBlockers.Count == 0;

        private static HashSet<BepInEx.BaseUnityPlugin> scoreSubmissionBlockers = new HashSet<BepInEx.BaseUnityPlugin>();

        public static void DisableScoreSubmission(BepInEx.BaseUnityPlugin plugin) => scoreSubmissionBlockers.Add(plugin);

        public static void EnableScoreSubmission(BepInEx.BaseUnityPlugin plugin) => scoreSubmissionBlockers.Remove(plugin);
    }
}
