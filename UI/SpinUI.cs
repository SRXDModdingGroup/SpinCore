using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace SpinCore.UI; 

/// <summary>
/// A utility class for creating new UI elements
/// </summary>
public static class SpinUI {
    /// <summary>
    /// Creates a new text object
    /// </summary>
    /// <param name="text">The text to display</param>
    /// <param name="parent">The transform to parent this object to</param>
    /// <returns>The new text object</returns>
    public static CustomTextMeshProUGUI CreateText(string text, Transform parent) {
        var gameObject = Object.Instantiate(BuildSettingsAsset.Instance.uiPrefabs.label, parent);
        var textComponent = gameObject.GetComponentInChildren<CustomTextMeshProUGUI>();
            
        gameObject.name = text;
        Object.Destroy(gameObject.GetComponentInChildren<TranslatedTextMeshPro>());
        textComponent.SetText(text);
        
        return textComponent;
    }

    /// <summary>
    /// Creates a new button
    /// </summary>
    /// <param name="text">The text to display on the button</param>
    /// <param name="parent">The transform to parent this object to</param>
    /// <param name="width">The width of the button</param>
    /// <param name="height">The height of the button</param>
    /// <param name="onClick">An optional action to invoke when the button is pressed</param>
    /// <returns>The new button</returns>
    public static Button CreateButton(string text, Transform parent, float width = 260f, float height = 30f, UnityAction onClick = null) {
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

    /// <summary>
    /// Creates a new toggle
    /// </summary>
    /// <param name="text">The text to display beside the toggle</param>
    /// <param name="parent">The transform to parent this object to</param>
    /// <param name="initialValue">The initial value of the toggle</param>
    /// <param name="width">The width of the toggle</param>
    /// <param name="onValueChanged">An optional action to invoke when the toggle's value is changed</param>
    /// <returns>The new toggle</returns>
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

    /// <summary>
    /// Creates a new slider
    /// </summary>
    /// <param name="text">The text to display above the slider</param>
    /// <param name="parent">The transform to parent this object to</param>
    /// <param name="min">The minimum value of the slider</param>
    /// <param name="max">The maximum value of the slider</param>
    /// <param name="initialValue">The initial value of the slider</param>
    /// <param name="wholeNumbers">If true, slider values will be rounded to integer ticks</param>
    /// <param name="width">The width of the slider</param>
    /// <param name="onValueChanged">An optional action to invoke when the slider's value is changed</param>
    /// <param name="valueDisplay">An optional function that returns a value to be displayed in the slider's text</param>
    /// <returns>The new slider</returns>
    public static Slider CreateSlider(string text, Transform parent, float min, float max, float initialValue = float.NaN, bool wholeNumbers = false, float width = 260f, UnityAction<float> onValueChanged = null, Func<float, string> valueDisplay = null) {
        var gameObject = Object.Instantiate(BuildSettingsAsset.Instance.uiPrefabs.slider, parent);
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
            textComponent.SetText($"{text}: {valueDisplay(initialValue)}");
        }

        return slider;
    }

    /// <summary>
    /// Creates a new dropdown menu
    /// </summary>
    /// <param name="text">The text to display above the dropdown</param>
    /// <param name="parent">The transform to parent this object to</param>
    /// <param name="options">The list of options to choose from</param>
    /// <param name="width">The width of the dropdown</param>
    /// <param name="onValueChanged">An optional action to invoke when the dropdown's value is changed</param>
    /// <returns>The new dropdown menu</returns>
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
    /// <summary>
    /// Creates a new dropdown menu, with its options being generated from an enum
    /// </summary>
    /// <param name="text">The text to display above the dropdown</param>
    /// <param name="parent">The transform to parent this object to</param>
    /// <param name="width">The width of the dropdown</param>
    /// <param name="onValueChanged">An optional action to invoke when the dropdown's value is changed</param>
    /// <typeparam name="T">The type of the enum</typeparam>
    /// <returns>The new dropdown menu</returns>
    public static TMP_Dropdown CreateDropdown<T>(string text, Transform parent, float width = 260f, UnityAction<int> onValueChanged = null) where T : Enum
        => CreateDropdown(text, parent, Enum.GetNames(typeof(T)), width, onValueChanged);

    /// <summary>
    /// Creates a new text input field
    /// </summary>
    /// <param name="name">The name of the input field</param>
    /// <param name="parent">The transform to parent this object to</param>
    /// <param name="initialValue">The initial value of the input field</param>
    /// <param name="characterValidation">The character types allowed by the input field</param>
    /// <param name="width">The width of the input field</param>
    /// <param name="onEndEdit">An optional action to invoke when the user is done editing the input field's value</param>
    /// <returns>The new input field</returns>
    public static TMP_InputField CreateInputField(string name, Transform parent, string initialValue = "", TMP_InputField.CharacterValidation characterValidation = TMP_InputField.CharacterValidation.None, float width = 250f, UnityAction<string> onEndEdit = null) {
        var gameObject = Object.Instantiate(BuildSettingsAsset.Instance.uiPrefabs.textField, parent);
        var layoutElement = gameObject.AddComponent<LayoutElement>();
        var inputField = gameObject.GetComponentInChildren<TMP_InputField>();

        gameObject.name = name;
        inputField.characterValidation = characterValidation;
        layoutElement.preferredWidth = width;
        layoutElement.minWidth = width;
        inputField.SetTextWithoutNotify(initialValue);
            
        if (onEndEdit != null)
            inputField.onEndEdit.AddListener(onEndEdit);

        return inputField;
    }
}