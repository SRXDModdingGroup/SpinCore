using SpinCore.Handlers.UI;
using System;
using UnityEngine;

namespace SpinCore.Behaviours
{
    public class XDModdedMenu : SpinMenu
    {
        //this is just here incase anyone wishes to do more with the actual SpinMenu 

        public CustomSpinMenu CurrentCustomSpinMenu;
        protected override void Update()
        {
            base.Update();
 
        }


        protected override void Awake()
        {
            base.Awake();
        }


        public override void CloseMenu(bool immediate = true)
        {
            foreach(var subMenu in subMenus)
            {
                subMenu.CloseMenu(true);
            }
            base.CloseMenu(true);
        }


    }

}

