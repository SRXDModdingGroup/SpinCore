using System;
using HarmonyLib;
using SMU.Utilities;
using SpinCore.Handlers;
using SpinCore.Handlers.UI;
using SpinCore.Patches;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace SpinCore {
    public static class MenuManager {
        private static bool initialized;
        private static Transform mainMenuContainer;

        public static void OpenMenuGroup(CustomSpinMenuGroup menuGroup) => menuGroup.Open();

        public static CustomSpinMenuGroup AddMenuGroup(string name) {
            var menuGroup = Object.Instantiate(UITemplates.MenuGroupTemplate, mainMenuContainer).GetComponent<CustomSpinMenuGroup>();
            
            menuGroup.Init(name);

            return menuGroup;
        }

        public static CustomTextMeshProUGUI CreateText(string text, Transform parent) {
            var gameObject = Object.Instantiate(BuildSettingsAsset.Instance.uiPrefabs.button, parent);
            var textComponent = gameObject.GetComponentInChildren<CustomTextMeshProUGUI>();
            
            gameObject.name = text;
            Object.Destroy(gameObject.GetComponentInChildren<TranslatedTextMeshPro>());
            textComponent.SetText(text);

            return textComponent;
        }

        public static TMP_InputField CreateInputField(string name, Transform parent, string initialValue, UnityAction<string> onValueChanged, float width = 250f) {
            var gameObject = Object.Instantiate(BuildSettingsAsset.Instance.uiPrefabs.textField, parent);
            var layoutElement = gameObject.AddComponent<LayoutElement>();
            var inputField = gameObject.GetComponentInChildren<TMP_InputField>();

            gameObject.name = name;
            layoutElement.preferredWidth = width;
            layoutElement.minWidth = width;
            inputField.SetTextWithoutNotify(initialValue);
            inputField.onValueChanged.AddListener(onValueChanged);

            return inputField;
        }

        public static Button CreateButton(string text, Transform parent, UnityAction onClick, float height = 30f, float width = 260f) {
            var gameObject = Object.Instantiate(BuildSettingsAsset.Instance.uiPrefabs.button, parent);
            var layoutElement = gameObject.AddComponent<LayoutElement>();
            var textTransform = gameObject.transform.Find("Imgtop").Find("Text TMP");
            var button = gameObject.GetComponentInChildren<Button>();
            
            gameObject.name = text;
            layoutElement.preferredHeight = height;
            layoutElement.minHeight = height;
            layoutElement.preferredWidth = width;
            layoutElement.minWidth = width;
            Object.Destroy(textTransform.GetComponent<TranslatedTextMeshPro>());
            textTransform.GetComponent<CustomTextMeshProUGUI>().SetText(text);
            button.onClick.AddListener(onClick);

            return button;
        }

        public static Toggle CreateToggle(string text, Transform parent, bool initialValue, UnityAction<bool> onValueChanged, float width = 260f) {
            var gameObject = Object.Instantiate(BuildSettingsAsset.Instance.togglePrefab, parent);
            var layoutElement = gameObject.AddComponent<LayoutElement>();
            var textTransform = gameObject.transform.Find("Heading");
            var toggle = gameObject.transform.Find("Toggle").GetComponent<Toggle>();
            
            gameObject.name = text;
            gameObject.transform.localScale = Vector3.one;
            layoutElement.preferredWidth = width;
            layoutElement.minWidth = width;
            Object.Destroy(textTransform.GetComponent<TranslatedTextMeshPro>());
            textTransform.GetComponent<CustomTextMeshProUGUI>().SetText(text);
            toggle.SetIsOnWithoutNotify(initialValue);
            toggle.onValueChanged.AddListener(onValueChanged);

            return toggle;
        }

        public static Slider CreateSlider(string text, Transform parent, float initialValue, float min, float max, UnityAction<float> onValueChanged, bool wholeNumbers = false, Func<float, string> valueDisplay = null, float width = 260f) {
            var gameObject = Object.Instantiate(BuildSettingsAsset.Instance.sliderPrefab, parent);
            var layoutElement = gameObject.GetComponentInChildren<LayoutElement>();
            var textTransform = gameObject.transform.Find("Heading");
            var slider = gameObject.transform.Find("SensitivitySlider").GetComponentInChildren<Slider>();
            var textComponent = textTransform.GetComponentInChildren<CustomTextMeshProUGUI>();

            gameObject.name = text;
            layoutElement.preferredWidth = width;
            layoutElement.minWidth = width;
            Object.Destroy(textTransform.GetComponentInChildren<TranslatedTextMeshPro>());
            slider.minValue = min;
            slider.maxValue = max;
            slider.wholeNumbers = wholeNumbers;
            slider.SetValueWithoutNotify(initialValue);
            slider.onValueChanged.AddListener(onValueChanged);

            if (valueDisplay == null)
                textComponent.SetText(text);
            else {
                slider.onValueChanged.AddListener(value => textComponent.SetText($"{text}: {valueDisplay(value)}"));
                textComponent.SetText($"{text}: {valueDisplay(min)}");
            }

            return slider;
        }

        public static TMP_Dropdown CreateDropdown(string text, Transform parent, UnityAction<int> onValueChanged, string[] options = null, float width = 260f) {
            var gameObject = Object.Instantiate(BuildSettingsAsset.Instance.dropdownPrefab, parent);
            var layoutElement = gameObject.AddComponent<LayoutElement>();
            var textTransform = gameObject.transform.Find("Heading");
            var dropdown = gameObject.transform.Find("Dropdown").GetComponent<TMP_Dropdown>();

            gameObject.name = text;
            gameObject.transform.localScale = Vector3.one;
            layoutElement.preferredWidth = width;
            layoutElement.minWidth = width;
            Object.Destroy(textTransform.GetComponent<TranslatedTextMeshPro>());
            textTransform.GetComponent<CustomTextMeshProUGUI>().SetText(text);
            dropdown.ClearOptions();
            dropdown.onValueChanged.AddListener(onValueChanged);

            if (options == null)
                return dropdown;
            
            foreach (string option in options)
                dropdown.options.Add(new TMP_Dropdown.OptionData(option));

            return dropdown;
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
            modsButton.onSelect.AddListener(delegate
            {
                foreach (var component in modsButton.textsToSet)
                {
                    component.SetText("Modded Options", true, 0.02f, StockMarketText.CaseType.ToLower, StockMarketText.ScrollType.ScrollNever);
                }
            });

            modsButton.button.onClick.AddListener(delegate
            {
                SpinCoreMenu.ModMenu.Open();
                SpinCoreMenu.ModMenu.GameStateToChangeToOnExitPress = "MainMenu";
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
            
            Dispatcher.QueueForNextFrame(() => {
                UITemplates.GenerateMenuTemplates(mainMenuContainer);
            });
            
            initialized = true;
        }

        private static void CreateMenuObjects(Transform mainMenuContainer)
        {
            foreach (CustomSpinMenuGroup menu in CustomSpinMenuHandler.MenuList)
            {
                // int gameStateint = gameStateDictionaryNameLookup.GetValueSafe(menu.menuID);

                var currentGameState = GameStatePatches.MenuIDToGameState.GetValueSafe(menu.Name);

                menu.SpinMenuGroupObject = Object.Instantiate(UITemplates.MenuGroupTemplate, mainMenuContainer);
                menu.SpinMenuGroupObject.SetActive(false);
                
                var newSpinMenuGroup = menu.SpinMenuGroupObject.GetComponent<SpinMenuGroup>();
                
                menu.SpinMenuGroupObject.name = menu.SpinMenuGroupName;
                newSpinMenuGroup.name = menu.SpinMenuGroupName;
                newSpinMenuGroup.menuType = (GameStateManager.GameState)(menu.GameStateValue);
                newSpinMenuGroup.SetProperty("gameState", currentGameState);
                newSpinMenuGroup.gameState.menuGroup = newSpinMenuGroup;
                
                var newModMenu = menu.SpinMenuGroupObject.transform.Find("ExampleMenu").gameObject;
                XDOptionsMenu oldXDOptionsMenu = newModMenu.GetComponent<XDOptionsMenu>();
                SpinMenu newSpinMenu = newModMenu.AddComponent<XDModdedMenu>();
                
                newModMenu.name = $"XD{menu.Name}Menu";
                
                newSpinMenuGroup.menus[0] = newSpinMenu;
                newSpinMenu.menuGroup = newSpinMenuGroup;

                var container = newModMenu.gameObject.transform.Find("Container");
                var contentArea = container.Find("ContentArea");
                var options = contentArea.Find("TopPanel").Find("Options");

                container.position -= new Vector3(0.5f, 0f, 0f);
                options.GetComponentInChildren<TranslatedTextMeshPro>().text.SetText(menu.Name);
                options.GetComponentInChildren<TranslatedTextMeshPro>().enabled = false;
                options.gameObject.name = menu.Name;

                var backButton = newModMenu.transform.Find("XDBackButton").GetComponentInChildren<Button>();
                
                backButton.onClick.AddListener(newSpinMenu.ExitButtonPressed);
                InstanceHandler.SharedMenuMusicInstance.menusToActiveMusic = InstanceHandler.SharedMenuMusicInstance.menusToActiveMusic.AddToArray(newSpinMenu);
                
                menu.SpinMenuGroupObject.SetActive(true);
                SMU.Events.EventHelper.InvokeAll(menu.OnMenuCreateAction);

                newSpinMenu.SetContentNavigation(backButton, null);
            }
        }
    }
}