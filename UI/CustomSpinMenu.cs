using System.Collections.Generic;
using HarmonyLib;
using SpinCore.Handlers;
using UnityEngine;
using UnityEngine.UI;

namespace SpinCore.UI; 

/// <summary>
/// Behavior that extends the functionality of menus
/// </summary>
public sealed class CustomSpinMenu : MonoBehaviour {
    /// <summary>
    /// The menu group that this menu belongs to
    /// </summary>
    public CustomSpinMenuGroup MenuGroup { get; private set; }
    
    /// <summary>
    /// The transform that this menu's UI elements should be parented to
    /// </summary>
    public Transform UIRoot { get; private set; }
    
    internal SpinMenu BaseSpinMenu { get; private set; }
        
    private Transform tabButtonContainer;
    private Dictionary<string, CustomSpinTab> tabs;
    private Dictionary<string, CustomContextMenu> contextMenus;

    /// <summary>
    /// Opens the tab with a given name
    /// </summary>
    /// <param name="name">The name of the tab to open</param>
    public void OpenTab(string name) {
        if (tabs.TryGetValue(name, out var tab))
            OpenTab(tab);
    }

    /// <summary>
    /// Opens the context menu with a given name
    /// </summary>
    /// <param name="name">The name of the context menu to open</param>
    public void OpenContextMenu(string name) {
        if (contextMenus.TryGetValue(name, out var contextMenu))
            contextMenu.Open();
    }

    /// <summary>
    /// Attempts to get a tab with the given name
    /// </summary>
    /// <param name="name">The name of the tab</param>
    /// <param name="tab">The found tab</param>
    /// <returns>True if the tab was found</returns>
    public bool TryGetTab(string name, out CustomSpinTab tab) => tabs.TryGetValue(name, out tab);

    /// <summary>
    /// Attempts to get a context menu with the given name
    /// </summary>
    /// <param name="name">The name of the context menu</param>
    /// <param name="contextMenu">The found context menu</param>
    /// <returns>True if the context menu was found</returns>
    public bool TryGetContextMenu(string name, out CustomContextMenu contextMenu) => contextMenus.TryGetValue(name, out contextMenu);

    /// <summary>
    /// Creates a new tab that is attached to this menu
    /// </summary>
    /// <param name="name">The name of the tab</param>
    /// <param name="createButton">If true, creates a button on the left side of the menu that opens the tab when pressed</param>
    /// <returns>The new tab</returns>
    public CustomSpinTab CreateTab(string name, bool createButton = true) {
        var tab = Instantiate(UITemplates.TabTemplate, UIRoot).GetComponent<CustomSpinTab>();
            
        tab.Init(name, this);
        tabs.Add(name, tab);

        if (createButton)
            SpinUI.CreateButton(name, tabButtonContainer, 240f, 30f, () => OpenTab(tab));
            
        if (tabs.Count == 1)
            tab.Open();
        else
            tab.Close();

        return tab;
    }

    /// <summary>
    /// Creates a new context menu that is attached to this menu
    /// </summary>
    /// <param name="name">The name of the context menu</param>
    /// <returns>The new context menu</returns>
    public CustomContextMenu CreateContextMenu(string name) {
        return null;
        
        // var contextMenu = BaseSpinMenu.GenerateContextMenu().gameObject.AddComponent<CustomContextMenu>();
        //     
        // contextMenu.Init(name);
        // contextMenus.Add(name, contextMenu);
        //
        // return contextMenu;
    }
        
    internal void Init(string name, CustomSpinMenuGroup menuGroup, bool isSubMenu) {
        // var backButton = transform.Find("XDBackButton").GetComponentInChildren<Button>();
        //     
        // gameObject.name = name;
        // MenuGroup = menuGroup;
        // UIRoot = transform.Find("Container").Find("ContentArea").Find("Content");
        // tabButtonContainer = transform.Find("TabListRoot").Find("Scroll View").Find("Viewport").Find("Content");
        // tabs = new Dictionary<string, CustomSpinTab>();
        // contextMenus = new Dictionary<string, CustomContextMenu>();
        // BaseSpinMenu = GetComponent<SpinMenu>();
        // BaseSpinMenu.isSubMenu = isSubMenu;
        // BaseSpinMenu.menuGroup = menuGroup.BaseMenuGroup;
        // backButton.onClick = new Button.ButtonClickedEvent();
        // backButton.onClick.AddListener(BaseSpinMenu.ExitButtonPressed);
        // InstanceHandler.SharedMenuMusic.menusToActiveMusic
        //     = InstanceHandler.SharedMenuMusic.menusToActiveMusic.AddToArray(BaseSpinMenu);
    }

    private void OpenTab(CustomSpinTab tab) {
        foreach (var pair in tabs)
            pair.Value.Close();
            
        tab.Open();
    }
}