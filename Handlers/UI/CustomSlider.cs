using SMU.Utilities;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpinCore.Handlers.UI
{
    public class CustomSlider
    {
        public GameObject UIElementGameObject; 
        public CustomTextMeshProUGUI HeadingTextMeshProUGUI;
        public string HeadingText { set { HeadingTextMeshProUGUI.text = value; } get { return HeadingTextMeshProUGUI.text; } }
        public Slider SliderElement;
        public float Min { get { return SliderElement.minValue; } set { SliderElement.minValue = value; } }
        public float Max { get { return SliderElement.maxValue; } set { SliderElement.maxValue = value; } }
        public bool UseWholeNumbers { get { return SliderElement.wholeNumbers; } set { SliderElement.wholeNumbers = value; } }

        private IReadOnlyBindable<float> Active => ActiveInternal;
        internal Bindable<float> ActiveInternal { get; }

        private Func<float, string> stringChangeFunc;

        public LayoutElement UILayoutElement;

        protected void SetupSlider(string sliderHeadingText, float min, float max, Func<float, string> funcTitleChange,  Transform parent, float width)
        {
            UIElementGameObject = GameObject.Instantiate(BuildSettingsAsset.Instance.uiPrefabs.slider, parent);
            UIElementGameObject.name = "SpinCoreObject" + sliderHeadingText;
            Transform text = UIElementGameObject.transform.Find("Heading");
            UILayoutElement = UIElementGameObject.GetComponentInChildren<LayoutElement>();
            UILayoutElement.preferredWidth = width;
            UILayoutElement.minWidth = width;

            SliderElement = UIElementGameObject.transform.Find("SensitivitySlider").GetComponentInChildren<Slider>();
            GameObject.Destroy(text.GetComponentInChildren<TranslatedTextMeshPro>());
            HeadingTextMeshProUGUI = text.GetComponentInChildren<CustomTextMeshProUGUI>();
            Min = min;
            Max = max;
            SliderElement.onValueChanged.AddListener(DropDownOptionChange);
            if(funcTitleChange == null)
            {
                HeadingText = sliderHeadingText;
            }
            else
            {
                stringChangeFunc = funcTitleChange;
                HeadingText = stringChangeFunc(ActiveInternal.Value);
            }
        }

        private void DropDownOptionChange(float value)
        {
            if(stringChangeFunc != null)
                HeadingText = stringChangeFunc(value);
            ActiveInternal.Value = value;
        }

        public CustomSlider(Func<float, string> stringChangeFunc, float min, float max, Bindable<float> bindable, Transform parent, float width = 260)
        {
            ActiveInternal = bindable;
            SetupSlider(string.Empty, min, max, stringChangeFunc, parent, width);
        }

        public CustomSlider(string headingText, float min, float max, Bindable<float> bindable, Transform parent, float width = 260)
        {
            ActiveInternal = bindable;
            SetupSlider(headingText, min, max, null, parent, width);
        }

        public CustomSlider(Func<float, string> stringChangeFunc, float min, float max, Bindable<float> bindable, CustomSpinTab spinTab, float width = 260)
        {
            ActiveInternal = bindable;
            SetupSlider(string.Empty, min, max, stringChangeFunc, spinTab.ContainerTransform, width);
        }
        public CustomSlider(string sliderHeadingText, float min, float max, Bindable<float> bindable, CustomSpinTab spinTab, float width = 260)
        {
            ActiveInternal = bindable;
            SetupSlider(sliderHeadingText, min, max, null, spinTab.ContainerTransform, width);
        }

        public CustomSlider(Func<float, string> stringChangeFunc, float min, float max, Bindable<float> bindable, CustomContextMenu contextMenu, float width = 260)
        {
            ActiveInternal = bindable;
            SetupSlider(string.Empty, min, max, stringChangeFunc, contextMenu.ContainerTransform, width);
        }
        public CustomSlider(string sliderHeadingText, float min, float max, Bindable<float> bindable, CustomContextMenu contextMenu, float width = 260)
        {
            ActiveInternal = bindable;
            SetupSlider(sliderHeadingText, min, max, null, contextMenu.ContainerTransform, width);
        }
    }
}
