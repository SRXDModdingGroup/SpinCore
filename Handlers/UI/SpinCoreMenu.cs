using System;

namespace SpinCore.Handlers.UI
{
    public class SpinCoreMenu
    {
        public static CustomSpinMenu ModMenu = new CustomSpinMenu("Modded Options");

        public static CustomContextMenu CustomLevelsOptionsContextMenu;
        public static CustomContextMenu OfficialLevelsOptionsContextMenu;


        public static Action OnCreateTabs;

        public static void InitialiseMenu()
        {
            ModMenu.OnMenuCreateAction += delegate
            {
                CustomSpinTab spinCoreOptions = ModMenu.CreateSpinTab("SpinCore Options");
                CustomMenuText spinCoreOptionsTitle = new CustomMenuText("SpinCore Options", spinCoreOptions);
                spinCoreOptionsTitle.UILayoutElement.preferredHeight = 30;
                spinCoreOptionsTitle.HeadingTextMeshProUGUI.fontSizeMax = 60;
                string spinCoreText = "SpinCore doesn't actually do anything yet...<br><br><br>Donate to me on patreon if you can! <3<br>https://www.patreon.com/moddingpink";
                CustomMenuText spinCoreOptionsText = new CustomMenuText(spinCoreText, spinCoreOptions);
                spinCoreOptionsText.UILayoutElement.preferredHeight = 60;
                spinCoreOptionsText.HeadingTextMeshProUGUI.fontSizeMax = 25;

                CustomSpinTab modList = ModMenu.CreateSpinTab("Mod List");
                CustomMenuText modListTitle = new CustomMenuText("Mod List", modList);
                modListTitle.UILayoutElement.preferredHeight = 30;
                modListTitle.HeadingTextMeshProUGUI.fontSizeMax = 60;

                string modListText = string.Empty;

                foreach (var plugin in BepInEx.Bootstrap.Chainloader.PluginInfos)
                {
                    CustomMenuText modText = new CustomMenuText($"{plugin.Value.Metadata.Name} [{plugin.Value.Metadata.Version}]", modList);
                    modText.UILayoutElement.preferredHeight = 20;
                    modText.HeadingTextMeshProUGUI.fontSizeMax = 20;
                }
                SMU.Events.EventHelper.InvokeAll(OnCreateTabs);

            };

        }
    }
}
