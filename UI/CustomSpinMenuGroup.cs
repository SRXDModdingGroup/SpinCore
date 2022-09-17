using System.Collections.Generic;
using System.Linq;
using SMU.Reflection;
using UnityEngine;

namespace SpinCore.UI; 

/// <summary>
/// Behavior that extends the functionality of menu groups
/// </summary>
public sealed class CustomSpinMenuGroup : MonoBehaviour {
    private static int gameStateCounter = 100;
        
    /// <summary>
    /// The primary menu in this menu group
    /// </summary>
    public CustomSpinMenu RootMenu { get; private set; }

    internal int GameStateValue { get; private set; }
    internal SpinMenuGroup BaseMenuGroup { get; private set; }

    private Dictionary<string, CustomSpinMenu> menus;

    /// <summary>
    /// Attempts to get a menu with the given name
    /// </summary>
    /// <param name="name">The name of the menu</param>
    /// <param name="menu">The found menu</param>
    /// <returns>True if the menu was found</returns>
    public bool TryGetMenu(string name, out CustomSpinMenu menu) => menus.TryGetValue(name, out menu);

    /// <summary>
    /// Creates a new sub menu belonging to this menu group
    /// </summary>
    /// <param name="name">The name of the sub menu</param>
    /// <returns>The new sub menu</returns>
    public CustomSpinMenu CreateSubMenu(string name) => CreateMenu(name, true);
    
    internal void Init(string name, GameState gameState) {
        gameObject.name = name;
        BaseMenuGroup = GetComponent<SpinMenuGroup>();
        GameStateValue = gameStateCounter;
        BaseMenuGroup.menuType = (GameStateManager.GameState) GameStateValue;
        gameStateCounter++;
        menus = new Dictionary<string, CustomSpinMenu>();
        RootMenu = CreateMenu("Root", false);
        BaseMenuGroup.RegisterMenus();
        BaseMenuGroup.SetProperty("gameState", gameState);
        gameState.menuGroup = BaseMenuGroup;
    }

    internal void Open(string fromState) {
        RootMenu.BaseSpinMenu.gameStateToChangeToOnExitPress = fromState;
        GameStateManager.Instance.ChangeState((GameStateManager.GameState) GameStateValue);
    }

    private CustomSpinMenu CreateMenu(string name, bool isSubMenu) {
        var menu = Instantiate(UITemplates.MenuTemplate, transform).GetComponent<CustomSpinMenu>();
            
        menu.Init(name, this, isSubMenu);
        menus.Add(name, menu);
        BaseMenuGroup.menus = menus.Select(pair => pair.Value.BaseSpinMenu).ToArray();

        return menu;
    }
}