using System.Collections.Generic;
using System.Collections.ObjectModel;
using SMU.Reflection;
using SMU.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace SpinCore.UI {
    public static class MenuManager {
        public static ReadOnlyDictionary<string, CustomSpinMenuGroup> MenuGroups { get; private set; }
        
        internal static CustomSpinMenuGroup ModOptionsGroup { get; private set; }

        private static bool initialized;
        private static Transform mainMenuContainer;
        private static Transform gameStateContainer;
        private static Dictionary<string, CustomSpinMenuGroup> menuGroups;

        public static void OpenMenuGroup(CustomSpinMenuGroup menuGroup, string fromState) => menuGroup.Open(fromState);
        public static void OpenMenuGroup(string name, string fromState) {
            if (menuGroups.TryGetValue(name, out var menuGroup))
                menuGroup.Open(fromState);
        }

        public static CustomSpinMenuGroup AddMenuGroup(string name) {
            var menuGroup = Object.Instantiate(UITemplates.MenuGroupTemplate, mainMenuContainer).GetComponent<CustomSpinMenuGroup>();
            var gameState = Object.Instantiate(UITemplates.GameStateTemplate, gameStateContainer).GetComponent<GameState>();

            gameState.gameObject.name = name;
            GameStateManager.Instance.rootGameState.SetupChildren(gameStateContainer.GetComponent<GameState>(), gameStateContainer);
            menuGroup.Init(name, gameState);
            menuGroups.Add(name, menuGroup);
            
            return menuGroup;
        }

        internal static void Initialize(XDMainMenu mainMenu) {
            if (initialized)
                return;
            
            var buttonsContainer = mainMenu.transform.Find("TopContainer").Find("ButtonsContainer");
            var modsButtonObject = Object.Instantiate(buttonsContainer.Find("ArcadeXDButton").gameObject, buttonsContainer);
            var modsButton = modsButtonObject.GetComponentInChildren<XDButton>();
            
            // Move all buttons up to fit the new, extra button
            buttonsContainer.position += new Vector3(-0.15f, 0.5f, 0f);
            
            // Creates the button, and moves it into place
            modsButtonObject.name = "ModdedXDButton";
            modsButtonObject.transform.position -= new Vector3(-1.025f, 3f, 0f);
            
            Object.Destroy(modsButtonObject.GetComponentInChildren<TranslatedTextMeshPro>());
            modsButtonObject.GetComponentInChildren<CustomTextMeshProUGUI>().text = "Mods";
            
            // Apply the text setter to mouse hover
            modsButton.onSelect.RemoveAllListeners();
            modsButton.onSelect.AddListener(delegate {
                foreach (var component in modsButton.textsToSet)
                    component.SetText("Modded Options", true, 0.02f, StockMarketText.CaseType.ToLower, StockMarketText.ScrollType.ScrollNever);
            });

            // Swap the buttons around
            var exitButtonTransform = buttonsContainer.Find("ExitXDButton");
                
            (exitButtonTransform.position, modsButtonObject.transform.position)
                = (modsButtonObject.transform.position, exitButtonTransform.position);

            // Fix the navigation
            var exitButton = buttonsContainer.Find("ExitXDButton").GetComponentInChildren<Button>();
            var navigation = exitButton.navigation;
            
            navigation.selectOnUp = modsButtonObject.GetComponentInChildren<Button>();
            exitButton.navigation = navigation;
            mainMenuContainer = mainMenu.transform.parent.parent;
            gameStateContainer = GameStateManager.Instance.rootGameState.transform.Find("WorldMenu");
            menuGroups = new Dictionary<string, CustomSpinMenuGroup>();
            MenuGroups = new ReadOnlyDictionary<string, CustomSpinMenuGroup>(menuGroups);

            Dispatcher.QueueForNextFrame(() => {
                UITemplates.GenerateMenuTemplates(mainMenuContainer, gameStateContainer);
                CreateModOptionsMenu();
                modsButton.button.onClick.RemoveAllListeners();
                modsButton.button.onClick.AddListener(() => OpenMenuGroup(ModOptionsGroup, "MainMenu"));
            });
            
            initialized = true;
        }

        private static void CreateModOptionsMenu() {
            ModOptionsGroup = AddMenuGroup("ModOptions");
        }
    }
}