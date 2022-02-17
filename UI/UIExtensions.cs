using System;
using System.Collections.Generic;
using SMU.Utilities;
using TMPro;
using UnityEngine;
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
            property.BindAndInvoke(toggle.SetIsOnWithoutNotify);
            toggle.onValueChanged.AddListener(value => property.Value = value);

            return toggle;
        }

        public static Slider Bind(this Slider slider, Bindable<float> property) {
            property.BindAndInvoke(slider.SetValueWithoutNotify);
            slider.onValueChanged.AddListener(value => property.Value = value);

            return slider;
        }
        public static Slider Bind(this Slider slider, Bindable<int> property) {
            property.BindAndInvoke(value => slider.SetValueWithoutNotify(Mathf.RoundToInt(value)));
            slider.onValueChanged.AddListener(value => property.Value = Mathf.RoundToInt(value));

            return slider;
        }

        public static TMP_Dropdown Bind(this TMP_Dropdown dropdown, Bindable<int> property) {
            property.BindAndInvoke(dropdown.SetValueWithoutNotify);
            dropdown.onValueChanged.AddListener(value => property.Value = value);

            return dropdown;
        }
        public static TMP_Dropdown Bind(this TMP_Dropdown dropdown, Bindable<string> property) {
            property.BindAndInvoke(value => dropdown.SetValueWithoutNotify(dropdown.IndexOf(value)));
            dropdown.onValueChanged.AddListener(value => property.Value = dropdown.options[value].text);

            return dropdown;
        }
        public static TMP_Dropdown Bind<T>(this TMP_Dropdown dropdown, Bindable<T> property) where T : Enum {
            property.BindAndInvoke(value => dropdown.SetValueWithoutNotify(Convert.ToInt32(value)));
            dropdown.onValueChanged.AddListener(value => property.Value = (T) Enum.GetValues(typeof(T)).GetValue(value));

            return dropdown;
        }
        public static TMP_Dropdown Bind<T>(this TMP_Dropdown dropdown, Bindable<T> property, IList<T> mapping) {
            property.BindAndInvoke(value => dropdown.SetValueWithoutNotify(mapping.IndexOf(value)));
            dropdown.onValueChanged.AddListener(value => property.Value = mapping[value]);

            return dropdown;
        }
        public static TMP_Dropdown Bind<T>(this TMP_Dropdown dropdown, Bindable<T> property, IDictionary<string, T> mapping) {
            property.BindAndInvoke(value => {
                int index = 0;

                for (int i = 0; i < dropdown.options.Count; i++) {
                    if (!mapping.TryGetValue(dropdown.options[i].text, out var item) || !item.Equals(value))
                        continue;
                
                    index = i;

                    break;
                }
                
                dropdown.SetValueWithoutNotify(index);
            });
            
            dropdown.onValueChanged.AddListener(value => {
                if (mapping.TryGetValue(dropdown.options[value].text, out var item))
                    property.Value = item;
            });

            return dropdown;
        }
        
        public static TMP_InputField Bind(this TMP_InputField inputField, Bindable<string> property) {
            property.Bind(inputField.SetTextWithoutNotify);
            inputField.onEndEdit.AddListener(value => property.Value = value);

            return inputField;
        }
    }
}