using System;
using System.Collections.Generic;
using SMU.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SpinCore.UI {
    public static class UIExtensions {
        public static int IndexOf(this TMP_Dropdown dropdown, string value) {
            var options = dropdown.options;
            
            for (int i = 0; i < options.Count; i++) {
                if (options[i].text == value)
                    return i;
            }

            return -1;
        }

        public static Toggle Bind(this Toggle toggle, Bindable<bool> property) {
            Bind(toggle.onValueChanged, property,
                value => toggle.isOn = value,
                value => property.Value = value);

            return toggle;
        }

        public static Slider Bind(this Slider slider, Bindable<float> property) {
            Bind(slider.onValueChanged, property,
                value => slider.value = value,
                value => property.Value = value);

            return slider;
        }
        public static Slider Bind(this Slider slider, Bindable<int> property) {
            Bind(slider.onValueChanged, property,
                value => slider.value = Mathf.RoundToInt(value),
                value => property.Value = Mathf.RoundToInt(value));

            return slider;
        }

        public static TMP_Dropdown Bind(this TMP_Dropdown dropdown, Bindable<int> property) {
            Bind(dropdown.onValueChanged, property,
                value => dropdown.value = value,
                value => property.Value = value);

            return dropdown;
        }
        public static TMP_Dropdown Bind(this TMP_Dropdown dropdown, Bindable<string> property) {
            Bind(dropdown.onValueChanged, property,
                value => dropdown.value = dropdown.IndexOf(value),
                value => property.Value = dropdown.options[value].text);

            return dropdown;
        }
        public static TMP_Dropdown Bind<T>(this TMP_Dropdown dropdown, Bindable<T> property) where T : Enum {
            Bind(dropdown.onValueChanged, property,
                value => dropdown.value = Convert.ToInt32(value),
                value => property.Value = (T) Enum.GetValues(typeof(T)).GetValue(value));

            return dropdown;
        }
        public static TMP_Dropdown Bind<T>(this TMP_Dropdown dropdown, Bindable<T> property, IList<T> mapping) {
            Bind(dropdown.onValueChanged, property,
                value => dropdown.value = mapping.IndexOf(value),
                value => property.Value = mapping[value]);

            return dropdown;
        }
        public static TMP_Dropdown Bind<T>(this TMP_Dropdown dropdown, Bindable<T> property, IDictionary<string, T> mapping) {
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

            return dropdown;
        }
        
        public static TMP_InputField Bind(this TMP_InputField inputField, Bindable<string> property) {
            Bind(inputField.onEndEdit, property,
                value => inputField.text = value,
                value => property.Value = value);

            return inputField;
        }

        private static void Bind<TEvent, TBindable>(UnityEvent<TEvent> UIEvent, Bindable<TBindable> property, Action<TBindable> UISetter, Action<TEvent> propertySetter) {
            bool locked = false;
            
            property.BindAndInvoke(value => {
                if (locked)
                    return;

                locked = true;
                UISetter(value);
                locked = false;
            });
            UIEvent.AddListener(value => {
                if (locked)
                    return;

                locked = true;
                propertySetter(value);
                locked = false;
            });
        }
    }
}