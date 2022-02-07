using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpinCore.Handlers.UI
{

    public class CustomSpinTab
    {
        public CustomSpinMenu customSpinMenu;

        List<GameObject> MenuTabObjects = new List<GameObject>();

        public Transform ContainerTransform;

        public SpinMenuTab currentTab;

        public bool isOpen = false;

        public Action OnMenuClose;
        public Action OnMenuOpen;
        public void OpenMenu()
        {
            if (!isOpen)
            {
                SMU.Events.EventHelper.InvokeAll(OnMenuOpen);
                currentTab.OpenMenu(true);
                isOpen = true;
            }
        }

        public void CloseMenu()
        {
            SMU.Events.EventHelper.InvokeAll(OnMenuClose);
            currentTab.CloseMenu(true);
            isOpen = false;

        }


        public CustomSpinTab(CustomSpinMenu constructorMenu, string tabName, bool createButton = true)
        {

            customSpinMenu = constructorMenu;
            customSpinMenu.spinTabs.Add(this);
            Transform content = customSpinMenu.spinMenuGroupObject.transform.Find(customSpinMenu.spinMenuName).Find("Container").Find("ContentArea").Find("Content");
            GameObject menuTab = GameObject.Instantiate(UICreationHandler.ExampleTab.gameObject, content);
            menuTab.transform.localScale = new Vector3(1f, 1f, 1f);
            menuTab.name = "CustomTab" + tabName;
            Transform container = menuTab.transform.Find("Scroll View").Find("Viewport").Find("Content");
            ContainerTransform = container;
            currentTab = menuTab.GetComponent<SpinMenuTab>();
            currentTab.InAmount = 0f;
            currentTab.isSubMenu = true;
            currentTab.menuGroup = customSpinMenu.moddedSpinMenu.menuGroup;

            if (createButton)
            {
                CustomButton button = new CustomButton(tabName, customSpinMenu.SideTabsTransform, delegate { customSpinMenu.OpenSpinTab(this); }, 30, 240);
            }



        }


    }

}
