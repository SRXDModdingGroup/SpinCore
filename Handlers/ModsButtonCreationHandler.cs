using SMU.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace SpinCore.Handlers
{
    internal static class ModsButtonCreationHandler
    {
        public static Button CreateModsButton(XDLevelSelectMenuBase levelSelect)
        {
            var customFolderButton = levelSelect.sortButton.gameObject;
            var backingTransform = customFolderButton.transform.parent.parent;
            var modsButton = Object.Instantiate(customFolderButton.transform.parent.gameObject, backingTransform);
            
            modsButton.name = "ModsButton";
            
            var buttonTransform = modsButton.transform.Find("SortButtonImg");
            
            buttonTransform.name = "ModsButtonImg";
            buttonTransform.Find("Imgtop").Find("ImagePlay").gameObject.GetComponent<Image>().sprite = ImageHelper.LoadSpriteFromResources("SpinCore.Images.ModsIcon.png");
            
            var textObj = buttonTransform.transform.Find("Imgtop").Find("Text TMP");
            
            Object.Destroy(textObj.GetComponent<TranslatedTextMeshPro>());
            textObj.GetComponent<CustomTextMeshProUGUI>().SetText("Mods");

            return buttonTransform.GetComponentInChildren<Button>();
        }
    }
}
