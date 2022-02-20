using System.Collections.Generic;

namespace SpinCore.Utility
{
    /// <summary>
    /// Contains utility functions for enabling and disabling score submissions
    /// </summary>
    public static class ScoreSubmissionUtility
    {
        /// <summary>
        /// True when score submission is enabled
        /// </summary>
        public static bool IsScoreSubmissionEnabled => scoreSubmissionBlockers.Count == 0;

        private static HashSet<BepInEx.BaseUnityPlugin> scoreSubmissionBlockers = new HashSet<BepInEx.BaseUnityPlugin>();

        /// <summary>
        /// Adds a plugin to the list of score submission blockers
        /// </summary>
        /// <param name="plugin">The plugin to add</param>
        public static void DisableScoreSubmission(BepInEx.BaseUnityPlugin plugin) => scoreSubmissionBlockers.Add(plugin);

        /// <summary>
        /// Removes a plugin from the list of score submission blockers
        /// </summary>
        /// <param name="plugin">The plugin to remove</param>
        public static void EnableScoreSubmission(BepInEx.BaseUnityPlugin plugin) => scoreSubmissionBlockers.Remove(plugin);
    }
}
