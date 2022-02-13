using SpinCore.Handlers.UI;
using UnityEngine;

namespace SpinCore.Handlers
{
    internal static class UITemplates {
        public static GameObject MenuGroupTemplate { get; private set; }
        public static GameObject MenuTemplate { get; private set; }
        public static GameObject TabTemplate { get; private set; }
        
        public static void GenerateMenuTemplates(Transform mainMenuContainer) {
            var objectPool = new GameObject();
            
            // Create pool
            objectPool.SetActive(false);
            objectPool.name = "MenuObjectTemplates";

            // Duplicate the Options Menu
            MenuGroupTemplate = Object.Instantiate(mainMenuContainer.Find("XDOptionsMenuGroup").gameObject);
            MenuGroupTemplate.name = "MenuGroup";
            MenuGroupTemplate.transform.SetParent(objectPool.transform);
            MenuGroupTemplate.AddComponent<CustomSpinMenuGroup>();
            MenuTemplate = MenuGroupTemplate.transform.Find("XDOptionsMenu").gameObject;
            MenuTemplate.gameObject.name = "Menu";
            MenuTemplate.transform.SetParent(objectPool.transform);
            MenuTemplate.AddComponent<CustomSpinMenu>();
            
            var contentArea = MenuTemplate.transform.Find("Container").Find("ContentArea");
            var content = contentArea.Find("Content");
            
            // Take the accessibility tab and make it our example tab
            TabTemplate = content.Find("Accessibility Tab").gameObject;
            TabTemplate.name = "Tab";
            TabTemplate.transform.SetParent(objectPool.transform);
            TabTemplate.transform.localScale = Vector3.one;
            TabTemplate.AddComponent<CustomSpinTab>();

            var tab = TabTemplate.GetComponent<SpinMenuTab>();
            var tabRectTransform = content.GetComponent<RectTransform>();
            var tabContent = TabTemplate.transform.Find("Scroll View").Find("Viewport").Find("Content");

            tab.InAmount = 0f;
            tab.isSubMenu = true;
            
            // Add padding to the menus
            tabRectTransform.offsetMin += new Vector2(20f, 0f);
            tabRectTransform.offsetMax += new Vector2(0f, -20f);

            // YAH YEET
            Object.Destroy(MenuTemplate.transform.Find("TrackInputPreview").gameObject);
            Object.Destroy(contentArea.Find("Support Content").gameObject);
            Object.Destroy(content.Find("General Tab").gameObject);
            Object.Destroy(content.Find("Visual Tab").gameObject);
            Object.Destroy(content.Find("Colors Tab").gameObject);
            Object.Destroy(content.Find("Audio Tab").gameObject);
            Object.Destroy(content.Find("Input Tab").gameObject);
            Object.Destroy(content.Find("Tabs").gameObject);
            Object.Destroy(tabContent.Find("Check Boxes Group").gameObject);
            Object.Destroy(tabContent.Find("Buttons Group").gameObject);
            Object.Destroy(tabContent.Find("Track Speed Group").gameObject);
            Object.Destroy(tabContent.Find("Note Beam Group").gameObject);
            Object.Destroy(tabContent.Find("Track Lines Group").gameObject);
            Object.Destroy(tabContent.Find("Defaults Button ").gameObject);

            var tabListRoot = Object.Instantiate(TabTemplate, MenuTemplate.transform);
            
            tabListRoot.name = "TabListRoot";
            tabListRoot.transform.localScale = Vector3.one;
            Object.Destroy(tabListRoot.GetComponent<SpinMenuTab>());
            
            var tabListRectTransform = tabListRoot.GetComponent<RectTransform>();
            
            tabListRectTransform.offsetMax = new Vector3(-40f, 460f);
            tabListRectTransform.offsetMin = new Vector3(-330f, 20f);
        }
    }
}
