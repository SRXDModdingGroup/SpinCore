using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpinCore.Handlers.UI
{
    public class CustomSpinTab : MonoBehaviour {
        private SpinMenuTab baseSpinTab;
        
        internal void Init(string name, CustomSpinMenu menu) {
            gameObject.name = name;
            baseSpinTab = GetComponent<SpinMenuTab>();
            baseSpinTab.menuGroup = menu.MenuGroup.BaseMenuGroup;
        }

        internal void Open() => baseSpinTab.OpenMenu(true);

        internal void Close() => baseSpinTab.CloseMenu();
    }
}
