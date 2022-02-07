using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpinCore.Handlers
{
    public class InstanceHandler
    {
        public static XDCustomLevelSelectMenu XDCustomLevelSelectMenuInstance;
        public static XDLevelSelectMenu XDLevelSelectMenuInstance;
        public static SharedMenuMusic SharedMenuMusicInstance;

        public static Action OnCustomLevelsOpen;
        public static Action OnOfficialLevelsOpen;
        public static Action OnSelectWheelChangeValue;
    }
}
