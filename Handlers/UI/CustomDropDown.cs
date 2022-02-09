using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using SMU.Reflection;
using SMU.Utilities;
using UnityEngine.EventSystems;

namespace SpinCore.Handlers.UI
{
    public class CustomDropDown
    {
        protected int CurrentID = 0;
        public struct DropDownOption
        {
            public DropDownOption(string optionName, int optionID)
            {
                OptionData = new TMP_Dropdown.OptionData();
                OptionID = optionID;
                OptionName = optionName;
            }

            public TMP_Dropdown.OptionData OptionData;
            public string OptionName { set { OptionData.text = value; } get { return OptionData.text; } }
            public int OptionID;
        }

        public GameObject UIElementGameObject;

        public CustomTextMeshProUGUI HeadingTextMeshProUGUI;

        public TMP_Dropdown UIDropDown;
        public string HeadingText { set { HeadingTextMeshProUGUI.text = value; } get { return HeadingTextMeshProUGUI.text; } }

        public List<DropDownOption> OptionList = new List<DropDownOption>();
        private IReadOnlyBindable<DropDownOption> Active => ActiveInternal;
        internal Bindable<DropDownOption> ActiveInternal { get; }

        public LayoutElement UILayoutElement;

        protected void SetupDropDown(string dropDownName, Transform parent, float width)
        {
            UIElementGameObject = GameObject.Instantiate(BuildSettingsAsset.Instance.dropdownPrefab, parent);
            UIElementGameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            UIElementGameObject.name = "SpinCoreObject" + dropDownName;
            UILayoutElement = UIElementGameObject.AddComponent<LayoutElement>();
            UILayoutElement.preferredWidth = width;
            UILayoutElement.minWidth = width;
            Transform text = UIElementGameObject.transform.Find("Heading");
            GameObject.Destroy(text.GetComponent<TranslatedTextMeshPro>());
            HeadingTextMeshProUGUI = text.GetComponent<CustomTextMeshProUGUI>();
            HeadingText = dropDownName;
            UIDropDown = UIElementGameObject.transform.Find("Dropdown").GetComponent<TMP_Dropdown>();
            UIDropDown.ClearOptions();
            UIDropDown.onValueChanged.AddListener(DropDownOptionChange);
            ScrollRect scrollRect = UIElementGameObject.transform.parent.parent.parent.GetComponent<ScrollRect>();
            if (scrollRect != null)
                scrollRect.onValueChanged.AddListener(delegate { UIDropDown.Hide(); });

        }

        private void DropDownOptionChange(int option)
        {
            if (OptionList.Count != 0)
                ActiveInternal.Value = OptionList[option];
        }

        private DropDownOption? GetFromList(int optionID)
        {
            foreach (var option in OptionList)
            {
                if (option.OptionID == optionID)
                    return option;
            }
            return null;
        }

        public int AddOption(string optionName)
        {
            int idToUse = CurrentID;
            if (!GetFromList(idToUse).HasValue) {
                DropDownOption option = new DropDownOption(optionName, CurrentID);
                OptionList.Add(option);
                UIDropDown.options.Add(option.OptionData);
                CurrentID++;
                return idToUse;
            }
            return -1;
        }

        public int RemoveOption(int optionID)
        {
            DropDownOption? option = GetFromList(optionID);
            if (option.HasValue) {
                OptionList.Remove(option.Value);
                UIDropDown.options.Remove(option.Value.OptionData);
                return option.Value.OptionID;
            }
            return -1;
        }

        public void AddOptions(string[] options)
        {
            if (options != null)
            {
                foreach (var option in options)
                {
                    AddOption(option);
                }
            }
        }

        public CustomDropDown(string dropDownName, Bindable<DropDownOption> bindable, Transform parent, string[] options = null, float width = 260f)
        {
            ActiveInternal = bindable;
            SetupDropDown(dropDownName, parent, width);
            if (options != null)
                AddOptions(options);
        }

        public CustomDropDown(string dropDownName, Bindable<DropDownOption> bindable, CustomSpinTab spinTab, string[] options = null, float width = 260f)
        {
            ActiveInternal = bindable;
            SetupDropDown(dropDownName, spinTab.ContainerTransform, width);
            if (options != null)
                AddOptions(options);
        }

        public CustomDropDown(string dropDownName, Bindable<DropDownOption> bindable, CustomContextMenu contextMenu, string[] options = null, float width = 260f)
        {
            ActiveInternal = bindable;
            SetupDropDown(dropDownName, contextMenu.ContainerTransform, width);
            if (options != null)
                AddOptions(options);
        }
    }
}
