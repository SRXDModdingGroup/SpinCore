using System;
using System.Collections.Generic;
using SMU.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SpinCore.UI; 

/// <summary>
/// Contains extension methods for UI elements
/// </summary>
public static class UIExtensions {
    /// <summary>
    /// Gets the index of an option in a dropdown menu
    /// </summary>
    /// <param name="dropdown">The dropdown menu</param>
    /// <param name="value">The value to find</param>
    /// <returns></returns>
    public static int IndexOf(this TMP_Dropdown dropdown, string value) {
        var options = dropdown.options;
            
        for (int i = 0; i < options.Count; i++) {
            if (options[i].text == value)
                return i;
        }

        return -1;
    }

    /// <summary>
    /// Binds the value of a toggle to a bindable property
    /// </summary>
    /// <param name="toggle">The toggle to bind</param>
    /// <param name="property">The property to bind</param>
    public static void Bind(this Toggle toggle, Bindable<bool> property) {
        Bind(toggle.onValueChanged, property,
            value => toggle.isOn = value,
            value => property.Value = value);
    }
        
    /// <summary>
    /// Binds the value of a slider to a bindable property
    /// </summary>
    /// <param name="slider">The slider to bind</param>
    /// <param name="property">The property to bind</param>
    public static void Bind(this Slider slider, Bindable<float> property) {
        Bind(slider.onValueChanged, property,
            value => slider.value = value,
            value => property.Value = value);
    }
    /// <summary>
    /// Binds the value of a slider to a bindable property
    /// </summary>
    /// <param name="slider">The slider to bind</param>
    /// <param name="property">The property to bind</param>
    public static void Bind(this Slider slider, Bindable<int> property) {
        Bind(slider.onValueChanged, property,
            value => slider.value = value,
            value => property.Value = Mathf.RoundToInt(value));
    }

    /// <summary>
    /// Binds the value of a dropdown to a bindable property, using the selected option's index
    /// </summary>
    /// <param name="dropdown">The dropdown to bind</param>
    /// <param name="property">The property to bind</param>
    public static void Bind(this TMP_Dropdown dropdown, Bindable<int> property) {
        Bind(dropdown.onValueChanged, property,
            value => dropdown.value = value,
            value => property.Value = value);
    }
    /// <summary>
    /// Binds the value of a dropdown to a bindable property, using the selected option's name
    /// </summary>
    /// <param name="dropdown">The dropdown to bind</param>
    /// <param name="property">The property to bind</param>
    public static void Bind(this TMP_Dropdown dropdown, Bindable<string> property) {
        Bind(dropdown.onValueChanged, property,
            value => dropdown.value = dropdown.IndexOf(value),
            value => property.Value = dropdown.options[value].text);
    }
    /// <summary>
    /// Binds the value of a dropdown to a bindable property, using an enum type that corresponds with the dropdown's options
    /// </summary>
    /// <param name="dropdown">The dropdown to bind</param>
    /// <param name="property">The property to bind</param>
    /// <typeparam name="T">The enum type</typeparam>
    public static void Bind<T>(this TMP_Dropdown dropdown, Bindable<T> property) where T : Enum {
        Bind(dropdown.onValueChanged, property,
            value => dropdown.value = Convert.ToInt32(value),
            value => property.Value = (T) Enum.GetValues(typeof(T)).GetValue(value));
    }
    /// <summary>
    /// Binds the value of a dropdown to a bindable property, using a list that maps option indices to values
    /// </summary>
    /// <param name="dropdown">The dropdown to bind</param>
    /// <param name="property">The property to bind</param>
    /// <param name="mapping">The list of values to map</param>
    /// <typeparam name="T">The type of the values to map</typeparam>
    public static void Bind<T>(this TMP_Dropdown dropdown, Bindable<T> property, IList<T> mapping) {
        Bind(dropdown.onValueChanged, property,
            value => dropdown.value = mapping.IndexOf(value),
            value => property.Value = mapping[value]);
    }
    /// <summary>
    /// Binds the value of a dropdown to a bindable property, using a dictionary that maps option names to values
    /// </summary>
    /// <param name="dropdown">The dropdown to bind</param>
    /// <param name="property">The property to bind</param>
    /// <param name="mapping">The dictionary of option names to map</param>
    /// <typeparam name="T">The type of the values to map</typeparam>
    public static void Bind<T>(this TMP_Dropdown dropdown, Bindable<T> property, IDictionary<string, T> mapping) {
        Bind(dropdown.onValueChanged, property,
            value => {
                int index = 0;

                for (int i = 0; i < dropdown.options.Count; i++) {
                    if (!mapping.TryGetValue(dropdown.options[i].text, out var item) || !item.Equals(value))
                        continue;
                
                    index = i;

                    break;
                }

                dropdown.value = index;
            },
            value => {
                if (mapping.TryGetValue(dropdown.options[value].text, out var item))
                    property.Value = item;
            });
    }
        
    /// <summary>
    /// Binds the value of an input field to a bindable property
    /// </summary>
    /// <param name="inputField">The input field to bind</param>
    /// <param name="property">The property to bind</param>
    public static void Bind(this TMP_InputField inputField, Bindable<string> property) {
        Bind(inputField.onEndEdit, property,
            value => inputField.text = value,
            value => property.Value = value);
    }

    private static void Bind<TEvent, TBindable>(UnityEvent<TEvent> uiEvent, Bindable<TBindable> property, Action<TBindable> uiSetter, Action<TEvent> propertySetter)
        => UIBinding<TEvent, TBindable>.Create(uiEvent, property, uiSetter, propertySetter);
}