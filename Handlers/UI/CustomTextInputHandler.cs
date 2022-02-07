using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using SMU.Utilities;
using UnityEngine.UI;

namespace SpinCore.Handlers.UI
{

    public class CustomTextInput
    {
        public GameObject UIElementGameObject;

        public TMP_InputField InputFieldElement;

        public LayoutElement UILayoutElement;

        private IReadOnlyBindable<string> Active => ActiveInternal;
        internal Bindable<string> ActiveInternal { get; }
        protected void SetupInput(string inputHeadingText, Transform parent, float width)
        {
            UIElementGameObject = GameObject.Instantiate(BuildSettingsAsset.Instance.uiPrefabs.textField, parent);
            UIElementGameObject.name = "SpinCoreObject" + inputHeadingText;
            UILayoutElement = UIElementGameObject.AddComponent<LayoutElement>();
            UILayoutElement.preferredWidth = width;
            UILayoutElement.minWidth = width;
            InputFieldElement = UIElementGameObject.GetComponentInChildren<TMP_InputField>();
            InputFieldElement.onValueChanged.AddListener(InputValueChange);
        }

        public void InputValueChange(string value) {
            ActiveInternal.Value = value;
        }

        public CustomTextInput(string sliderHeadingText, Bindable<string> bindable, Transform parent, float width = 250)
        {
            ActiveInternal = bindable;
            SetupInput(sliderHeadingText, parent, width);
        }

        public CustomTextInput(string sliderHeadingText, Bindable<string> bindable, CustomSpinTab spinTab,  float width = 250)
        {
            ActiveInternal = bindable;
            SetupInput(sliderHeadingText, spinTab.ContainerTransform, width);
        }

        public CustomTextInput(string sliderHeadingText, Bindable<string> bindable, CustomContextMenu contextMenu, float width = 250)
        {
            ActiveInternal = bindable;
            SetupInput(sliderHeadingText, contextMenu.ContainerTransform, width);
        }
    }
}
