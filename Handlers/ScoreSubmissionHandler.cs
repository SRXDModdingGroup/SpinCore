using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SpinCore.Handlers
{
    public class ScoreSubmissionHandler
    {

        private static List<BepInEx.BaseUnityPlugin> scoreSubmission = new List<BepInEx.BaseUnityPlugin>();

        public static bool IsScoreSubmissionEnabled { get { return scoreSubmission.Count == 0; } }

        public static void DisableScoreSubmission(BepInEx.BaseUnityPlugin plugin)
        {
            if (!scoreSubmission.Contains(plugin)) { 
                scoreSubmission.Add(plugin);
            }
        }
        public static void EnableScoreSubmission(BepInEx.BaseUnityPlugin plugin)
        {
            if (scoreSubmission.Contains(plugin))
            {
                scoreSubmission.Remove(plugin);
            }
        }
    }
}
