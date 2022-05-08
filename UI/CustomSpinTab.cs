using UnityEngine;

namespace SpinCore.UI; 

/// <summary>
/// Behaviour that extends the functionality of tabs
/// </summary>
public class CustomSpinTab : MonoBehaviour {
    /// <summary>
    /// The transform that this tab's UI elements should be parented to
    /// </summary>
    public Transform UIRoot { get; private set; }
        
    private SpinMenuTab baseSpinTab;
        
    internal void Init(string name, CustomSpinMenu menu) {
        gameObject.name = name;
        baseSpinTab = GetComponent<SpinMenuTab>();
        baseSpinTab.menuGroup = menu.MenuGroup.BaseMenuGroup;
        UIRoot = transform.Find("Scroll View").Find("Viewport").Find("Content");
    }

    internal void Open() => baseSpinTab.OpenMenu(true);

    internal void Close() => baseSpinTab.CloseMenu();
}