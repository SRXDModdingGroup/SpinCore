using SMU.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace SpinCore.Handlers.UI
{

    public class CustomCheckbox
    {
        public GameObject UIElementGameObject;

        public CustomTextMeshProUGUI HeadingTextMeshProUGUI;

        public string HeadingText { set { HeadingTextMeshProUGUI.text = value; } get { return HeadingTextMeshProUGUI.text; } }

        private IReadOnlyBindable<bool> Active => ActiveInternal;
        internal Bindable<bool> ActiveInternal { get; }

        public LayoutElement UILayoutElement;

        protected void SetupCheckbox(string checkboxName, Transform parent, float width)
        {
            UIElementGameObject = GameObject.Instantiate(BuildSettingsAsset.Instance.togglePrefab, parent);
            UIElementGameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            UIElementGameObject.name = "SpinCoreObject" + checkboxName;
            UILayoutElement = UIElementGameObject.AddComponent<LayoutElement>();
            UILayoutElement.preferredWidth = width;
            UILayoutElement.minWidth = width;
            Transform text = UIElementGameObject.transform.Find("Heading");
            GameObject.Destroy(text.GetComponent<TranslatedTextMeshPro>());
            Transform checkBox = UIElementGameObject.transform.Find("Toggle");
            checkBox.name = "CheckBox";
            Toggle toggle = checkBox.GetComponent<Toggle>();
            HeadingTextMeshProUGUI = text.GetComponent<CustomTextMeshProUGUI>();
            HeadingText = checkboxName;
            toggle.isOn = ActiveInternal.Value;
            toggle.onValueChanged.AddListener(delegate { ActiveInternal.Value = toggle.isOn; });
        }


        public CustomCheckbox(string checkboxName, Bindable<bool> bindable, Transform parent, float width = 260f)
        {
            ActiveInternal = bindable;
            SetupCheckbox(checkboxName, parent, width);
        }

        public CustomCheckbox(string checkboxName, Bindable<bool> bindable, CustomSpinTab spinTab, float width = 260f)
        {
            ActiveInternal = bindable;
            SetupCheckbox(checkboxName, spinTab.ContainerTransform, width);
        }

        public CustomCheckbox(string checkboxName, Bindable<bool> bindable, CustomContextMenu contextMenu, float width = 260f)
        {
            ActiveInternal = bindable;
            SetupCheckbox(checkboxName, contextMenu.ContainerTransform, width);
        }
    }
}
