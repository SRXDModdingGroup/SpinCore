using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpinCore.Handlers.UI
{
    public class CustomSpinTab
    {
        public CustomSpinMenu CustomSpinMenu;

        List<GameObject> menuTabObjects = new List<GameObject>();

        public Transform ContainerTransform;

        public SpinMenuTab CurrentTab;

        public bool IsOpen = false;

        public Action OnMenuClose;
        public Action OnMenuOpen;
        public void OpenMenu()
        {
            if (!IsOpen)
            {
                SMU.Events.EventHelper.InvokeAll(OnMenuOpen);
                CurrentTab.OpenMenu(true);
                IsOpen = true;
            }
        }

        public void CloseMenu()
        {
            SMU.Events.EventHelper.InvokeAll(OnMenuClose);
            CurrentTab.CloseMenu(true);
            IsOpen = false;

        }


        public CustomSpinTab(CustomSpinMenu constructorMenu, string tabName, bool createButton = true)
        {

            CustomSpinMenu = constructorMenu;
            CustomSpinMenu.SpinTabs.Add(this);
            Transform content = CustomSpinMenu.SpinMenuGroupObject.transform.Find(CustomSpinMenu.SpinMenuName).Find("Container").Find("ContentArea").Find("Content");
            GameObject menuTab = GameObject.Instantiate(UICreationHandler.ExampleTab.gameObject, content);
            menuTab.transform.localScale = new Vector3(1f, 1f, 1f);
            menuTab.name = "CustomTab" + tabName;
            Transform container = menuTab.transform.Find("Scroll View").Find("Viewport").Find("Content");
            ContainerTransform = container;
            CurrentTab = menuTab.GetComponent<SpinMenuTab>();
            CurrentTab.InAmount = 0f;
            CurrentTab.isSubMenu = true;
            CurrentTab.menuGroup = CustomSpinMenu.ModdedSpinMenu.menuGroup;

            if (createButton)
            {
                CustomButton button = new CustomButton(tabName, CustomSpinMenu.SideTabsTransform, delegate { CustomSpinMenu.OpenSpinTab(this); }, 30, 240);
            }



        }


    }

}
