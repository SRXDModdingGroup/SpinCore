using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace SpinCore.UI {
    public class CustomSpinMenu : SpinMenu {
        public ReadOnlyDictionary<string, CustomSpinTab> Tabs { get; private set; }
        public ReadOnlyDictionary<string, CustomContextMenu> ContextMenus { get; private set; }
        public new CustomSpinMenuGroup MenuGroup { get; private set; }
        public Transform UIRoot { get; private set; }
        
        public override Vector2 MenuTransitionAnchorOffset => new Vector2(1.5f, 0.0f);

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
                SpinUI.CreateButton(name, tabListRoot, () => OpenTab(tab), 30, 240);

            return tab;
        }

        public CustomContextMenu CreateContextMenu(string name) {
            var contextMenu = GenerateContextMenu().gameObject.AddComponent<CustomContextMenu>();
            
            contextMenu.Init(name);
            contextMenus.Add(name, contextMenu);

            return contextMenu;
        }

        internal void Init(string name, CustomSpinMenuGroup menuGroup, bool isSubMenu) {
            gameObject.name = name;
            MenuGroup = menuGroup;
            UIRoot = transform.Find("Container").Find("ContentArea").Find("Content");
            tabListRoot = transform.Find("TabListRoot");
            tabs = new Dictionary<string, CustomSpinTab>();
            Tabs = new ReadOnlyDictionary<string, CustomSpinTab>(tabs);
            contextMenus = new Dictionary<string, CustomContextMenu>();
            ContextMenus = new ReadOnlyDictionary<string, CustomContextMenu>(contextMenus);
            this.isSubMenu = isSubMenu;
        }

        private void OpenTab(CustomSpinTab tab) {
            foreach (var pair in tabs)
                pair.Value.Close();
            
            tab.Open();
        }
    }
}