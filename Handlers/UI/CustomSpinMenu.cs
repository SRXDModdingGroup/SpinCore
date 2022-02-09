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
        public string MenuName;
        public int GameStateVal;
        public Action OnMenuCreateAction;
        public bool MenuHasBeenCreated = false;
        public string SpinMenuName { get { return $"XD{MenuName}Menu"; } }
        public string SpinMenuGroupName { get { return $"XD{MenuName}MenuGroup"; } }

        public GameObject SpinMenuGroupObject;

        public Transform SideTabsTransform;

        public List<CustomSpinTab> SpinTabs = new List<CustomSpinTab>();
        public XDModdedMenu ModdedSpinMenu
        {
            get
            {
                return SpinMenuGroupObject.transform.Find(SpinMenuName).transform.GetComponent<XDModdedMenu>();
            }
        }

        public string GameStateToChangeToOnExitPress
        {
            get
            {
                return ModdedSpinMenu.gameStateToChangeToOnExitPress;
            }
            set
            {
                ModdedSpinMenu.gameStateToChangeToOnExitPress = value;
            }
        }

        public GameState GameState
        {
            get
            {
                return ModdedSpinMenu.gameState;
            }
        }

        public CustomSpinMenu(string title)
        {
            MenuName = title;
            OnMenuCreateAction += delegate {
                SideTabsTransform = SpinMenuGroupObject.transform.Find(SpinMenuName).Find("SideTabListView").Find("Scroll View").Find("Viewport").Find("Content"); 
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
            foreach (CustomSpinTab tab in SpinTabs)
            {
                tab.CloseMenu();
            }
            customSpinTab.OpenMenu();
        }

        public void OpenMenu()
        {
            GameStateManager.Instance.ChangeState((GameStateManager.GameState)GameStateVal);
        }

        public void OpenMenu(CustomSpinTab customSpinTab)
        {
            GameStateManager.Instance.ChangeState((GameStateManager.GameState)GameStateVal);
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
                if (menu.MenuName == menuName)
                {
                    GameStateManager.Instance.ChangeState((GameStateManager.GameState)menu.GameStateVal);
                    break;
                }
            }
        }

        public static void OpenCustomSpinMenu(CustomSpinMenu menu)
        {
            GameStateManager.Instance.ChangeState((GameStateManager.GameState)menu.GameStateVal);
        }

    }
}
