using System;
using SMU.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace SpinCore.UI {
    public static class SpinUI {
        public static CustomTextMeshProUGUI CreateText(string text, Transform parent) {
            var gameObject = Object.Instantiate(BuildSettingsAsset.Instance.uiPrefabs.button, parent);
            var textComponent = gameObject.GetComponentInChildren<CustomTextMeshProUGUI>();
            
            gameObject.name = text;
            Object.Destroy(gameObject.GetComponentInChildren<TranslatedTextMeshPro>());
            textComponent.SetText(text);

            return textComponent;
        }

        public static Button CreateButton(string text, Transform parent, float height = 30f, float width = 260f, UnityAction onClick = null) {
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
            
            if (onClick != null)
                button.onClick.AddListener(onClick);

            return button;
        }

        public static Toggle CreateToggle(string text, Transform parent, bool initialValue = false, float width = 260f, UnityAction<bool> onValueChanged = null) {
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
            
            if (onValueChanged != null)
                toggle.onValueChanged.AddListener(onValueChanged);

            return toggle;
        }

        public static Slider CreateSlider(string text, Transform parent, float min, float max, float initialValue = float.NaN, bool wholeNumbers = false, float width = 260f, UnityAction<float> onValueChanged = null, Func<float, string> valueDisplay = null) {
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

            if (float.IsNaN(initialValue))
                initialValue = min;
            
            slider.SetValueWithoutNotify(initialValue);
            
            if (onValueChanged != null)
                slider.onValueChanged.AddListener(onValueChanged);

            if (valueDisplay == null)
                textComponent.SetText(text);
            else {
                slider.onValueChanged.AddListener(value => textComponent.SetText($"{text}: {valueDisplay(value)}"));
                textComponent.SetText($"{text}: {valueDisplay(min)}");
            }

            return slider;
        }

        public static TMP_Dropdown CreateDropdown(string text, Transform parent, string[] options = null, float width = 260f, UnityAction<int> onValueChanged = null) {
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
            
            if (onValueChanged != null)
                dropdown.onValueChanged.AddListener(onValueChanged);

            if (options == null)
                return dropdown;
            
            foreach (string option in options)
                dropdown.options.Add(new TMP_Dropdown.OptionData(option));

            return dropdown;
        }
        public static TMP_Dropdown CreateDropdown<T>(string text, Transform parent, float width = 260f, UnityAction<int> onValueChanged = null) where T : Enum
            => CreateDropdown(text, parent, Enum.GetNames(typeof(T)), width, onValueChanged);

        public static TMP_InputField CreateInputField(string name, Transform parent, string initialValue = "", float width = 250f, UnityAction<string> onEndEdit = null) {
            var gameObject = Object.Instantiate(BuildSettingsAsset.Instance.uiPrefabs.textField, parent);
            var layoutElement = gameObject.AddComponent<LayoutElement>();
            var inputField = gameObject.GetComponentInChildren<TMP_InputField>();

            gameObject.name = name;
            layoutElement.preferredWidth = width;
            layoutElement.minWidth = width;
            inputField.SetTextWithoutNotify(initialValue);
            
            if (onEndEdit != null)
                inputField.onEndEdit.AddListener(onEndEdit);

            return inputField;
        }
    }
}