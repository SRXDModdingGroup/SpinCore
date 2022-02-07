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

        public string text { set { HeadingTextMeshProUGUI.text = value; } get { return HeadingTextMeshProUGUI.text; } }

        protected void setupText(string textToUse, Transform parent)
        {
            UIElementGameObject = GameObject.Instantiate(BuildSettingsAsset.Instance.uiPrefabs.label, parent);
            UIElementGameObject.name = "SpinCoreObject" + textToUse;
            GameObject.Destroy(UIElementGameObject.GetComponentInChildren<TranslatedTextMeshPro>());
            HeadingTextMeshProUGUI = UIElementGameObject.GetComponentInChildren<CustomTextMeshProUGUI>();
            UILayoutElement = UIElementGameObject.GetComponentInChildren<LayoutElement>();
            text = textToUse;
        }

        public CustomMenuText(string textToUse, Transform parent)
        {
            setupText(textToUse, parent);
        }

        public CustomMenuText(string textToUse, CustomSpinTab spinTab)
        {
            setupText(textToUse, spinTab.ContainerTransform);
        }

        public CustomMenuText(string textToUse, CustomContextMenu contextMenu)
        {
            setupText(textToUse, contextMenu.ContainerTransform);
        }
    }
}
