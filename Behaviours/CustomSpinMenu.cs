using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

namespace SpinCore.Handlers.UI {
    public class CustomSpinMenu : MonoBehaviour {
        public ReadOnlyDictionary<string, CustomSpinTab> Tabs { get; private set; }
        
        public CustomSpinMenuGroup MenuGroup { get; private set; }

        private Transform tabRoot;
        private Transform tabListRoot;
        private Dictionary<string, CustomSpinTab> tabs;

        public CustomSpinTab CreateTab(string name, bool createButton = true) {
            var tab = Instantiate(UITemplates.TabTemplate, tabRoot).GetComponent<CustomSpinTab>();
            
            tab.Init(name, this);
            tabs.Add(name, tab);

            if (createButton)
                MenuManager.CreateButton(name, tabListRoot, () => OpenTab(tab), 30, 240);

            return tab;
        }

        public void OpenTab(string name) {
            if (tabs.TryGetValue(name, out var tab))
                OpenTab(tab);
        }
        
        internal void Init(string name, CustomSpinMenuGroup menuGroup) {
            gameObject.name = name;
            MenuGroup = menuGroup;
            tabRoot = transform.Find("Container").Find("ContentArea").Find("Content");
            tabListRoot = transform.Find("TabListRoot");
            tabs = new Dictionary<string, CustomSpinTab>();
            Tabs = new ReadOnlyDictionary<string, CustomSpinTab>(tabs);
        }

        private void OpenTab(CustomSpinTab tab) {
            foreach (var pair in tabs)
                pair.Value.Close();
            
            tab.Open();
        }
    }
}