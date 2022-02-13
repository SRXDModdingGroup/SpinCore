using System;

namespace SpinCore.Handlers.UI
{
    internal static class SpinCoreMenu
    {
        public static CustomSpinMenuGroup ModMenu = new CustomSpinMenuGroup("Modded Options");

        public static CustomContextMenu CustomLevelsOptionsContextMenu;
        public static CustomContextMenu OfficialLevelsOptionsContextMenu;
        public static Action OnCreateTabs;

        public static void InitialiseMenu()
        {
            ModMenu.OnMenuCreateAction += delegate
            {
                var spinCoreOptions = ModMenu.AddTab("SpinCore Options");
                var spinCoreOptionsTitle = new CustomMenuText("SpinCore Options", spinCoreOptions);
                spinCoreOptionsTitle.UILayoutElement.preferredHeight = 30;
                spinCoreOptionsTitle.HeadingTextMeshProUGUI.fontSizeMax = 60;
                string spinCoreText = "SpinCore doesn't actually do anything yet...<br><br><br>Donate to me on patreon if you can! <3<br>https://www.patreon.com/moddingpink";
                var spinCoreOptionsText = new CustomMenuText(spinCoreText, spinCoreOptions);
                spinCoreOptionsText.UILayoutElement.preferredHeight = 60;
                spinCoreOptionsText.HeadingTextMeshProUGUI.fontSizeMax = 25;

                var modList = ModMenu.AddTab("Mod List");
                var modListTitle = new CustomMenuText("Mod List", modList);
                modListTitle.UILayoutElement.preferredHeight = 30;
                modListTitle.HeadingTextMeshProUGUI.fontSizeMax = 60;

                string modListText = string.Empty;

                foreach (var plugin in BepInEx.Bootstrap.Chainloader.PluginInfos)
                {
                    var modText = new CustomMenuText($"{plugin.Value.Metadata.Name} [{plugin.Value.Metadata.Version}]", modList);
                    modText.UILayoutElement.preferredHeight = 20;
                    modText.HeadingTextMeshProUGUI.fontSizeMax = 20;
                }
                
                SMU.Events.EventHelper.InvokeAll(OnCreateTabs);
            };
        }
    }
}
