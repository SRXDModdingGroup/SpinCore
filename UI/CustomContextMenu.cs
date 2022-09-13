using UnityEngine;

namespace SpinCore.UI; 

/// <summary>
/// Behavior that extends the functionality of context menus
/// </summary>
public sealed class CustomContextMenu : MonoBehaviour {
    private SpinContextMenu baseContextMenu;

    internal void Init(string name) {
        gameObject.name = name;
        baseContextMenu = GetComponent<SpinContextMenu>();
        baseContextMenu.closeWhenClickingOutside = true;
            
        var optionsTransform = baseContextMenu.transform.Find("Container").Find("Background").Find("TopPanel").Find("Options");
        var menuTab = Instantiate(UITemplates.TabTemplate.gameObject, baseContextMenu.transform);
        var rectTrans = menuTab.GetComponent<RectTransform>();
            
        optionsTransform.name = "Heading";
        Destroy(optionsTransform.GetComponent<TranslatedTextMeshPro>());
        optionsTransform.GetComponent<CustomTextMeshProUGUI>().SetText(name);
        menuTab.transform.localScale = Vector3.one;
        menuTab.transform.position += new Vector3(-10f, 6.1f, 0f);
        menuTab.name = "ScrollableTab";
        rectTrans.offsetMax += new Vector2(-25f, 0f);
        rectTrans.offsetMin += new Vector2(5f, 20f);
    }

    internal void Open(bool immediate = false) => baseContextMenu.OpenMenu(immediate);
        
    internal void Close(bool immediate = false) => baseContextMenu.CloseMenu(immediate);
}