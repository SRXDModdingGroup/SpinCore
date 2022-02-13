using System;

namespace SpinCore.Handlers
{
    internal class InstanceHandler
    {
        public static XDCustomLevelSelectMenu XDCustomLevelSelectMenuInstance;
        public static XDLevelSelectMenu XDLevelSelectMenuInstance;
        public static SharedMenuMusic SharedMenuMusicInstance;

        public static Action OnCustomLevelsOpen;
        public static Action OnOfficialLevelsOpen;
        public static Action OnSelectWheelChangeValue;
    }
}
