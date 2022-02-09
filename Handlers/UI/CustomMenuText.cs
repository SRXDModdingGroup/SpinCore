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
    public class CustomMenuText
    {
        public GameObject UIElementGameObject;

        public CustomTextMeshProUGUI HeadingTextMeshProUGUI;

        public LayoutElement UILayoutElement;

        public string Text { set { HeadingTextMeshProUGUI.text = value; } get { return HeadingTextMeshProUGUI.text; } }

        protected void SetupText(string textToUse, Transform parent)
        {
            UIElementGameObject = GameObject.Instantiate(BuildSettingsAsset.Instance.uiPrefabs.label, parent);
            UIElementGameObject.name = "SpinCoreObject" + textToUse;
            GameObject.Destroy(UIElementGameObject.GetComponentInChildren<TranslatedTextMeshPro>());
            HeadingTextMeshProUGUI = UIElementGameObject.GetComponentInChildren<CustomTextMeshProUGUI>();
            UILayoutElement = UIElementGameObject.GetComponentInChildren<LayoutElement>();
            Text = textToUse;
        }

        public CustomMenuText(string textToUse, Transform parent)
        {
            SetupText(textToUse, parent);
        }

        public CustomMenuText(string textToUse, CustomSpinTab spinTab)
        {
            SetupText(textToUse, spinTab.ContainerTransform);
        }

        public CustomMenuText(string textToUse, CustomContextMenu contextMenu)
        {
            SetupText(textToUse, contextMenu.ContainerTransform);
        }
    }
}
