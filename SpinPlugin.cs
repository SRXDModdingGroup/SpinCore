﻿using BepInEx;
using SMU.Utilities;
using SpinCore.UI;

namespace SpinCore; 

/// <summary>
/// The base class for all plugins that utilize SpinCore
/// </summary>
public abstract class SpinPlugin : BaseUnityPlugin {
    /// <summary>
    /// Called when first initializing the plugin
    /// </summary>
    /// <remarks>Remember to call base.Awake when overriding this function</remarks>
    protected virtual void Awake() => MenuManager.RegisterSpinPlugin(this);

    /// <summary>
    /// Called by SpinCore when initializing menus for each plugin. Use this to create the options tab and any other menus for this plugin
    /// </summary>
    protected internal virtual void CreateMenus() { }

    /// <summary>
    /// Called after all mod menus have been initialized by SpinCore. Use this for any initialization that depends on other menus
    /// </summary>
    protected internal virtual void LateInit() { }
}