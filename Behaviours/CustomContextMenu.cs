using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpinCore.Handlers.UI
{
    public class CustomContextMenu : MonoBehaviour {
        private CustomSpinTab spinMenuTab;
        private SpinMenu spinMenu;
        public Transform ContainerTransform;

        public SpinContextMenu CurrentContexMenu;
        public bool IsOpen;
        public bool DoesExist;
        public CustomTextMeshProUGUI HeadingTextMeshProUGUI;

        public string HeadingText {
            set => HeadingTextMeshProUGUI.text = value;
            get => HeadingTextMeshProUGUI.text;
        }

        public Action OnMenuClose;
        public Action OnMenuOpen;

        public void OpenMenu(bool immediate = false) {

            CurrentContexMenu.OpenMenu(immediate);
            SMU.Events.EventHelper.InvokeAll(OnMenuOpen);
        }


        public void CloseMenu(bool immediate = false) {
            CurrentContexMenu.CloseMenu(immediate);
            SMU.Events.EventHelper.InvokeAll(OnMenuOpen);

        }

        private void SetUpContainer(string menuTitle, Vector3 movement) {
            var optionsTransform = CurrentContexMenu.transform.Find("Container").Find("Background").Find("TopPanel").Find("Options");
            optionsTransform.name = "HeadingText";
            Object.Destroy(optionsTransform.GetComponent<TranslatedTextMeshPro>());
            HeadingTextMeshProUGUI = optionsTransform.GetComponent<CustomTextMeshProUGUI>();
            HeadingText = menuTitle;


            var menuTab = Object.Instantiate(UITemplates.TabTemplate.gameObject, CurrentContexMenu.transform);
            menuTab.transform.localScale = new Vector3(1f, 1f, 1f);
            menuTab.transform.position += new Vector3(-10f, 6.1f, 0f);
            menuTab.name = "ScrollableTab";
            var scrollView = menuTab.transform.Find("Scroll View");
            ContainerTransform = scrollView.Find("Viewport").Find("Content");
            var rectTrans = menuTab.GetComponent<RectTransform>();
            rectTrans.offsetMax += new Vector2(-25, 0);
            rectTrans.offsetMin += new Vector2(5, 20);
            menuTab.transform.position += movement;
            CurrentContexMenu.closeWhenClickingOutside = true;
            CurrentContexMenu.onMenuClose += delegate { IsOpen = false; };
            CurrentContexMenu.onMenuOpen += delegate { IsOpen = true; };
            DoesExist = true;
        }

        public CustomContextMenu(string menuTitle, CustomSpinTab constructorMenu) {
            CurrentContexMenu = Object.Instantiate(BuildSettingsAsset.Instance.contextMenuPopupPrefab, constructorMenu.CustomSpinMenu.SpinMenuGroupObject.transform).GetComponentInChildren<SpinContextMenu>();
            CurrentContexMenu.name = "SpinCoreObject" + menuTitle;
            CurrentContexMenu.isSubMenu = true;
            constructorMenu.CustomSpinMenu.ModdedSpinMenu.subMenus.Add(CurrentContexMenu);

            spinMenuTab = constructorMenu;
            spinMenuTab.OnMenuClose += delegate { CurrentContexMenu.CloseMenu(); };
            SetUpContainer(menuTitle, new Vector3(0.12f, 0.47f, 0));
        }

        public CustomContextMenu(string menuTitle, SpinMenu constructorMenu) {

            spinMenu = constructorMenu;
            CurrentContexMenu = constructorMenu.GenerateContextMenu();
            CurrentContexMenu.name = "SpinCoreObject" + menuTitle;
            SetUpContainer(menuTitle, new Vector3(-0.06f, 0.5f, 0));

        }
    }
}
