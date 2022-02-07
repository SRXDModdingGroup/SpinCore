using System;
using UnityEngine;

namespace SpinCore.Handlers.UI
{
    public class CustomContextMenu
    {
        CustomSpinTab spinMenuTab;
        SpinMenu spinMenu;
        public Transform ContainerTransform;

        public SpinContextMenu CurrentContexMenu;
        public bool isOpen = false;
        public bool doesExist = false;
        public CustomTextMeshProUGUI HeadingTextMeshProUGUI;
        public string HeadingText { set { HeadingTextMeshProUGUI.text = value; } get { return HeadingTextMeshProUGUI.text; } }
        public Action OnMenuClose;
        public Action OnMenuOpen;
        public void OpenMenu(bool immediate = false)
        {

            CurrentContexMenu.OpenMenu(immediate);
            SMU.Events.EventHelper.InvokeAll(OnMenuOpen);
        }


        public void CloseMenu(bool immediate = false)
        {
            CurrentContexMenu.CloseMenu(immediate);
            SMU.Events.EventHelper.InvokeAll(OnMenuOpen);

        }

        void SetUpContainer(string menuTitle, Vector3 Movement)
        {
            Transform OptionsTransform = CurrentContexMenu.transform.Find("Container").Find("Background").Find("TopPanel").Find("Options");
            OptionsTransform.name = "HeadingText";
            GameObject.Destroy(OptionsTransform.GetComponent<TranslatedTextMeshPro>());
            HeadingTextMeshProUGUI = OptionsTransform.GetComponent<CustomTextMeshProUGUI>();
            HeadingText = menuTitle;


            GameObject menuTab = GameObject.Instantiate(UICreationHandler.ExampleTab.gameObject, CurrentContexMenu.transform);
            menuTab.transform.localScale = new Vector3(1f, 1f, 1f);
            menuTab.transform.position += new Vector3(-10f, 6.1f, 0f);
            menuTab.name = "ScrollableTab";
            Transform scrollView = menuTab.transform.Find("Scroll View");
            ContainerTransform = scrollView.Find("Viewport").Find("Content");
            RectTransform rectTrans = menuTab.GetComponent<RectTransform>();
            rectTrans.offsetMax += new Vector2(-25, 0);
            rectTrans.offsetMin += new Vector2(5, 20);
            menuTab.transform.position += Movement;
            CurrentContexMenu.closeWhenClickingOutside = true;
            CurrentContexMenu.onMenuClose += delegate { isOpen = false; };
            CurrentContexMenu.onMenuOpen += delegate { isOpen = true; };
            doesExist = true;


        }
        public CustomContextMenu(string menuTitle, CustomSpinTab constructorMenu)
        {
            CurrentContexMenu = UnityEngine.Object.Instantiate<GameObject>(BuildSettingsAsset.Instance.contextMenuPopupPrefab, constructorMenu.customSpinMenu.spinMenuGroupObject.transform).GetComponentInChildren<SpinContextMenu>();
            CurrentContexMenu.name = "SpinCoreObject" + menuTitle;
            CurrentContexMenu.isSubMenu = true;
            constructorMenu.customSpinMenu.moddedSpinMenu.subMenus.Add(CurrentContexMenu);

            spinMenuTab = constructorMenu;
            spinMenuTab.OnMenuClose += delegate { CurrentContexMenu.CloseMenu(); };
            SetUpContainer(menuTitle, new Vector3(0.12f, 0.47f, 0));

        }


        public CustomContextMenu(string menuTitle, SpinMenu constructorMenu)
        {

            spinMenu = constructorMenu;
            CurrentContexMenu = constructorMenu.GenerateContextMenu();
            CurrentContexMenu.name = "SpinCoreObject" + menuTitle;
            SetUpContainer(menuTitle, new Vector3(-0.06f, 0.5f, 0));

        }


    }
}
