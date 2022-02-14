using UnityEngine;

namespace SpinCore.UI
{
    public class CustomSpinTab : MonoBehaviour {
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
}
