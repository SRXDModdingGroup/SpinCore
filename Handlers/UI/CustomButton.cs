using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SpinCore.Handlers.UI
{

    public class CustomButton
    {
        public GameObject UIElementGameObject;

        public CustomTextMeshProUGUI HeadingTextMeshProUGUI;

        public Button Button;

        public LayoutElement UILayoutElement;
        public string HeadingText { set { HeadingTextMeshProUGUI.text = value; } get { return HeadingTextMeshProUGUI.text; } }

        protected void SetupButton(string buttonName, Transform parent, UnityAction onButtonPress, float height, float width)
        {
            UIElementGameObject = GameObject.Instantiate(BuildSettingsAsset.Instance.uiPrefabs.button, parent);
            UIElementGameObject.name = "SpinCoreObject" + buttonName;
            UILayoutElement = UIElementGameObject.AddComponent<LayoutElement>();
            UILayoutElement.preferredHeight = height;
            UILayoutElement.minHeight = height;
            UILayoutElement.preferredWidth = width;
            UILayoutElement.minWidth = width;
            Transform imgTopTransform = UIElementGameObject.transform.Find("Imgtop");
            Transform textTransform = imgTopTransform.Find("Text TMP");
            GameObject.Destroy(textTransform.GetComponent<TranslatedTextMeshPro>());
            HeadingTextMeshProUGUI = textTransform.GetComponent<CustomTextMeshProUGUI>();
            HeadingText = buttonName;
            Button = UIElementGameObject.GetComponentInChildren<Button>();
            if (onButtonPress != null)
                Button.onClick.AddListener(onButtonPress);
        }

        public CustomButton(string buttonName, Transform parent, UnityAction onButtonPress = null, float height = 30, float width = 260)
        {
            SetupButton(buttonName, parent, onButtonPress, height, width);
        }

        public CustomButton(string buttonName, CustomSpinTab spinTab, UnityAction onButtonPress = null, float height = 30, float width = 260)
        {
            SetupButton(buttonName, spinTab.ContainerTransform, onButtonPress, height, width);
        }

        public CustomButton(string buttonName, CustomContextMenu contextMenu, UnityAction onButtonPress = null, float height = 30, float width = 260)
        {
            SetupButton(buttonName, contextMenu.ContainerTransform, onButtonPress, height, width);
        }
    }
}
