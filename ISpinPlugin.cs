namespace SpinCore {
    public interface ISpinPlugin { 
        /// <summary>
        /// Called by SpinCore when initializing menus for each plugin. Use this to create the options tab and any other menus for this plugin
        /// </summary>
        void CreateMenus();

        /// <summary>
        /// Called after all mod menus have been initialized by SpinCore. Use this for any initialization that depends on other menus
        /// </summary>
        void LateInit();
    }
}