using SMU.Utilities;
using SpinCore.Handlers.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SpinCore.Handlers
{
    public class ModsButtonCreationHandler
    {

        public static Button CreateModsButton(XDLevelSelectMenuBase levelSelect)
        {
            GameObject customFolderButton = levelSelect.sortButton.gameObject;

            Transform backingTransform = customFolderButton.transform.parent.parent;

            GameObject modsButton = GameObject.Instantiate(customFolderButton.transform.parent.gameObject, backingTransform);
            modsButton.name = "ModsButton";
            Transform buttonTransform = modsButton.transform.Find("SortButtonImg");
            buttonTransform.name = "ModsButtonImg";
            buttonTransform.Find("Imgtop").Find("ImagePlay").gameObject.GetComponent<Image>().sprite = ImageHelper.LoadSpriteFromResources("SpinCore.Images.ModsIcon.png");
            Transform textObj = buttonTransform.transform.Find("Imgtop").Find("Text TMP");
            GameObject.Destroy(textObj.GetComponent<TranslatedTextMeshPro>());
            textObj.GetComponent<CustomTextMeshProUGUI>().SetText("Mods");

            return buttonTransform.GetComponentInChildren<Button>();

        }

    }
}
