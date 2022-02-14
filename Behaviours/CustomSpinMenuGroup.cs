using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace SpinCore.Handlers.UI
{
    public class CustomSpinMenuGroup : MonoBehaviour {
        private static int gameStateCounter = 100;
        
        public CustomSpinMenu RootMenu { get; private set; }
        public ReadOnlyDictionary<string, CustomSpinMenu> Menus { get; private set; }

        internal int GameStateValue { get; private set; }
        internal SpinMenuGroup BaseMenuGroup { get; private set; }

        private Dictionary<string, CustomSpinMenu> menus;

        public CustomSpinMenu CreateSubMenu(string name) {
            var menu = Instantiate(UITemplates.MenuTemplate, transform).GetComponent<CustomSpinMenu>();
            
            menu.Init(name, this, true);
            menus.Add(name, menu);

            return menu;
        }
        
        internal void Init(string name) {
            gameObject.name = name;
            BaseMenuGroup = GetComponent<SpinMenuGroup>();
            GameStateValue = gameStateCounter;
            BaseMenuGroup.menuType = (GameStateManager.GameState) GameStateValue;
            gameStateCounter++;
            menus = new Dictionary<string, CustomSpinMenu>();
            Menus = new ReadOnlyDictionary<string, CustomSpinMenu>(menus);
            
            RootMenu = Instantiate(UITemplates.MenuTemplate, transform).GetComponent<CustomSpinMenu>();
            RootMenu.Init("Root", this, false);
            menus.Add("Root", RootMenu);
        }

        internal void Open(string fromState) {
            RootMenu.BaseSpinMenu.gameStateToChangeToOnExitPress = fromState;
            GameStateManager.Instance.ChangeState((GameStateManager.GameState) GameStateValue);
        }
    }
}
