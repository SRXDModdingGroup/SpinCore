using SpinCore.Handlers.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace SpinCore.Handlers
{
    public class UICreationHandler
    {

        public static GameObject ExampleTab;
        public static GameObject ExampleMenuGroup;

        public static void GenerateMenuObjects(Transform mainMenuContainer)
        {
            GameObject ObjectPool = new GameObject();
            ObjectPool.SetActive(false);
            ObjectPool.name = "MenuExampleObjects";
            //create pool

            //duplicate the Options Menu
            ExampleMenuGroup = GameObject.Instantiate<UnityEngine.GameObject>(mainMenuContainer.Find("XDOptionsMenuGroup").gameObject);
            ExampleMenuGroup.name = "ExampleMenuGroup";
            ExampleMenuGroup.transform.SetParent(ObjectPool.transform);
            Transform ExampleMenu = ExampleMenuGroup.transform.Find("XDOptionsMenu");
            ExampleMenu.gameObject.name = "ExampleMenu";
            GameObject.DestroyImmediate(ExampleMenu.Find("TrackInputPreview").gameObject);
            Transform Container = ExampleMenu.Find("Container");
            Transform ContentArea = Container.Find("ContentArea");
            GameObject.DestroyImmediate(ContentArea.Find("Support Content").gameObject);
            Transform content = ContentArea.Find("Content");

            Transform ExampleTabTransform = content.Find("Accessibility Tab");
            ExampleTab = ExampleTabTransform.gameObject;
            ExampleTab.name = "ExampleTab";
            //take the accessability tab and make it our example tab
            ExampleTabTransform.SetParent(ObjectPool.transform);

            Transform ExampleTabContent = ExampleTab.transform.Find("Scroll View").Find("Viewport").Find("Content");
            RectTransform ExampleTabRectTrans = content.GetComponent<RectTransform>();
            ExampleTabRectTrans.offsetMin += new Vector2(20f, 0f);
            ExampleTabRectTrans.offsetMax += new Vector2(0f, -20f);
            //add padding to the menus

            GameObject.DestroyImmediate(ExampleTabContent.Find("Check Boxes Group").gameObject);
            GameObject.DestroyImmediate(ExampleTabContent.Find("Buttons Group").gameObject);
            GameObject.DestroyImmediate(ExampleTabContent.Find("Track Speed Group").gameObject);
            GameObject.DestroyImmediate(ExampleTabContent.Find("Note Beam Group").gameObject);
            GameObject.DestroyImmediate(ExampleTabContent.Find("Track Lines Group").gameObject);
            GameObject.DestroyImmediate(ExampleTabContent.Find("Defaults Button ").gameObject);
            GameObject.DestroyImmediate(content.Find("General Tab").gameObject);
            GameObject.DestroyImmediate(content.Find("Visual Tab").gameObject);
            GameObject.DestroyImmediate(content.Find("Colors Tab").gameObject);
            GameObject.DestroyImmediate(content.Find("Audio Tab").gameObject);
            GameObject.DestroyImmediate(content.Find("Input Tab").gameObject);
            GameObject.DestroyImmediate(content.Find("Tabs").gameObject);
            //YAH YEET

            GameObject menuTab = GameObject.Instantiate(UICreationHandler.ExampleTab.gameObject, ExampleMenu);
            menuTab.name = "SideTabListView";
            Transform menuTransform = menuTab.transform;
            menuTransform.localScale = new Vector3(1f, 1f, 1f);
            GameObject.DestroyImmediate(menuTab.GetComponent<SpinMenuTab>());
            RectTransform rectTrans = menuTab.GetComponent<RectTransform>();
            rectTrans.offsetMax = new Vector3(-40f, 460f);
            rectTrans.offsetMin = new Vector3(-330f, 20f);
            Transform menuTabContent = menuTab.transform.Find("Scroll View").Find("Viewport").Find("Content");



        }

    }
}
