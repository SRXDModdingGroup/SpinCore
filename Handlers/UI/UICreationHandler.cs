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
            GameObject objectPool = new GameObject();
            objectPool.SetActive(false);
            objectPool.name = "MenuExampleObjects";
            //create pool

            //duplicate the Options Menu
            ExampleMenuGroup = GameObject.Instantiate<UnityEngine.GameObject>(mainMenuContainer.Find("XDOptionsMenuGroup").gameObject);
            ExampleMenuGroup.name = "ExampleMenuGroup";
            ExampleMenuGroup.transform.SetParent(objectPool.transform);
            Transform exampleMenu = ExampleMenuGroup.transform.Find("XDOptionsMenu");
            exampleMenu.gameObject.name = "ExampleMenu";
            GameObject.DestroyImmediate(exampleMenu.Find("TrackInputPreview").gameObject);
            Transform container = exampleMenu.Find("Container");
            Transform contentArea = container.Find("ContentArea");
            GameObject.DestroyImmediate(contentArea.Find("Support Content").gameObject);
            Transform content = contentArea.Find("Content");

            Transform exampleTabTransform = content.Find("Accessibility Tab");
            ExampleTab = exampleTabTransform.gameObject;
            ExampleTab.name = "ExampleTab";
            //take the accessability tab and make it our example tab
            exampleTabTransform.SetParent(objectPool.transform);

            Transform exampleTabContent = ExampleTab.transform.Find("Scroll View").Find("Viewport").Find("Content");
            RectTransform exampleTabRectTrans = content.GetComponent<RectTransform>();
            exampleTabRectTrans.offsetMin += new Vector2(20f, 0f);
            exampleTabRectTrans.offsetMax += new Vector2(0f, -20f);
            //add padding to the menus

            GameObject.DestroyImmediate(exampleTabContent.Find("Check Boxes Group").gameObject);
            GameObject.DestroyImmediate(exampleTabContent.Find("Buttons Group").gameObject);
            GameObject.DestroyImmediate(exampleTabContent.Find("Track Speed Group").gameObject);
            GameObject.DestroyImmediate(exampleTabContent.Find("Note Beam Group").gameObject);
            GameObject.DestroyImmediate(exampleTabContent.Find("Track Lines Group").gameObject);
            GameObject.DestroyImmediate(exampleTabContent.Find("Defaults Button ").gameObject);
            GameObject.DestroyImmediate(content.Find("General Tab").gameObject);
            GameObject.DestroyImmediate(content.Find("Visual Tab").gameObject);
            GameObject.DestroyImmediate(content.Find("Colors Tab").gameObject);
            GameObject.DestroyImmediate(content.Find("Audio Tab").gameObject);
            GameObject.DestroyImmediate(content.Find("Input Tab").gameObject);
            GameObject.DestroyImmediate(content.Find("Tabs").gameObject);
            //YAH YEET

            GameObject menuTab = GameObject.Instantiate(UICreationHandler.ExampleTab.gameObject, exampleMenu);
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
