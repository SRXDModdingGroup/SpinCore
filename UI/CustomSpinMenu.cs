using System.Collections.Generic;
using System.Collections.ObjectModel;
using HarmonyLib;
using SpinCore.Handlers;
using UnityEngine;
using UnityEngine.UI;

namespace SpinCore.UI {
    public class CustomSpinMenu : MonoBehaviour {
        public ReadOnlyDictionary<string, CustomSpinTab> Tabs { get; private set; }
        public ReadOnlyDictionary<string, CustomContextMenu> ContextMenus { get; private set; }
        public CustomSpinMenuGroup MenuGroup { get; private set; }
        public Transform UIRoot { get; private set; }
        
        internal SpinMenu BaseSpinMenu { get; private set; }
        
        private Transform tabListRoot;
        private Dictionary<string, CustomSpinTab> tabs;
        private Dictionary<string, CustomContextMenu> contextMenus;

        public void OpenTab(string name) {
            if (tabs.TryGetValue(name, out var tab))
                OpenTab(tab);
        }

        public void OpenContextMenu(string name) {
            if (contextMenus.TryGetValue(name, out var contextMenu))
                contextMenu.Open();
        }

        public CustomSpinTab CreateTab(string name, bool createButton = true) {
            var tab = Instantiate(UITemplates.TabTemplate, UIRoot).GetComponent<CustomSpinTab>();
            
            tab.Init(name, this);
            tabs.Add(name, tab);

            if (createButton)
                SpinUI.CreateButton(name, tabListRoot, 30f, 240f, () => OpenTab(tab));
            
            if (tabs.Count == 1)
                tab.Open();
            else
                tab.Close();

            return tab;
        }

        public CustomContextMenu CreateContextMenu(string name) {
            var contextMenu = BaseSpinMenu.GenerateContextMenu().gameObject.AddComponent<CustomContextMenu>();
            
            contextMenu.Init(name);
            contextMenus.Add(name, contextMenu);

            return contextMenu;
        }
        
        internal void Init(string name, CustomSpinMenuGroup menuGroup, bool isSubMenu) {
            var backButton = transform.Find("XDBackButton").GetComponentInChildren<Button>();
            
            gameObject.name = name;
            MenuGroup = menuGroup;
            UIRoot = transform.Find("Container").Find("ContentArea").Find("Content");
            tabListRoot = transform.Find("TabListRoot").Find("Scroll View").Find("Viewport").Find("Content");
            tabs = new Dictionary<string, CustomSpinTab>();
            Tabs = new ReadOnlyDictionary<string, CustomSpinTab>(tabs);
            contextMenus = new Dictionary<string, CustomContextMenu>();
            ContextMenus = new ReadOnlyDictionary<string, CustomContextMenu>(contextMenus);
            BaseSpinMenu = GetComponent<SpinMenu>();
            BaseSpinMenu.isSubMenu = isSubMenu;
            BaseSpinMenu.menuGroup = menuGroup.BaseMenuGroup;
            backButton.onClick = new Button.ButtonClickedEvent();
            backButton.onClick.AddListener(BaseSpinMenu.ExitButtonPressed);
            InstanceHandler.SharedMenuMusic.menusToActiveMusic
                = InstanceHandler.SharedMenuMusic.menusToActiveMusic.AddToArray(BaseSpinMenu);
        }

        private void OpenTab(CustomSpinTab tab) {
            foreach (var pair in tabs)
                pair.Value.Close();
            
            tab.Open();
        }
    }
}