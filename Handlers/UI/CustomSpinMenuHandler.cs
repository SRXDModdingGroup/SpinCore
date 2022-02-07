using SpinCore.Behaviours;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SpinCore.Handlers.UICreationHandler;

namespace SpinCore.Handlers.UI
{


    public class CustomSpinMenu
    {
        public string menuName;
        public int gameStateVal;
        public Action onMenuCreateAction;
        public bool menuHasBeenCreated = false;
        public string spinMenuName { get { return $"XD{menuName}Menu"; } }
        public string spinMenuGroupName { get { return $"XD{menuName}MenuGroup"; } }

        public GameObject spinMenuGroupObject;

        public Transform SideTabsTransform;

        public List<CustomSpinTab> spinTabs = new List<CustomSpinTab>();
        public XDModdedMenu moddedSpinMenu
        {
            get
            {
                return spinMenuGroupObject.transform.Find(spinMenuName).transform.GetComponent<XDModdedMenu>();
            }
        }

        public string gameStateToChangeToOnExitPress
        {
            get
            {
                return moddedSpinMenu.gameStateToChangeToOnExitPress;
            }
            set
            {
                moddedSpinMenu.gameStateToChangeToOnExitPress = value;
            }
        }

        public GameState gameState
        {
            get
            {
                return moddedSpinMenu.gameState;
            }
        }

        public CustomSpinMenu(string title)
        {
            menuName = title;
            onMenuCreateAction += delegate {
                SideTabsTransform = spinMenuGroupObject.transform.Find(spinMenuName).Find("SideTabListView").Find("Scroll View").Find("Viewport").Find("Content"); 
            };
            CustomSpinMenuHandler.MenuList.Add(this);

           
        }

        public CustomSpinTab CreateSpinTab(string buttonName, bool createButton = true)
        {
            CustomSpinTab newSpinTab = new CustomSpinTab(this, buttonName, createButton);
            return newSpinTab;
        }

        public void OpenSpinTab(CustomSpinTab customSpinTab)
        {
            foreach (CustomSpinTab tab in spinTabs)
            {
                tab.CloseMenu();
            }
            customSpinTab.OpenMenu();
        }

        public void OpenMenu()
        {
            GameStateManager.Instance.ChangeState((GameStateManager.GameState)gameStateVal);
        }

        public void OpenMenu(CustomSpinTab customSpinTab)
        {
            GameStateManager.Instance.ChangeState((GameStateManager.GameState)gameStateVal);
            OpenSpinTab(customSpinTab);
        }
    }





    public class CustomSpinMenuHandler
    {

        public static List<CustomSpinMenu> MenuList = new List<CustomSpinMenu>();

        public static void OpenCustomSpinMenu(string menuName)
        {
            foreach (CustomSpinMenu menu in CustomSpinMenuHandler.MenuList)
            {
                if (menu.menuName == menuName)
                {
                    GameStateManager.Instance.ChangeState((GameStateManager.GameState)menu.gameStateVal);
                    break;
                }
            }
        }

        public static void OpenCustomSpinMenu(CustomSpinMenu menu)
        {
            GameStateManager.Instance.ChangeState((GameStateManager.GameState)menu.gameStateVal);
        }

    }
}
