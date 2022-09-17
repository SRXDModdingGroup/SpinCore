using System;
using SMU.Utilities;
using UnityEngine.Events;

namespace SpinCore.UI;

internal sealed class UIBinding<TEvent, TBindable> {
    private bool locked;
    private Action<TBindable> uiSetter;
    private Action<TEvent> propertySetter;

    private UIBinding(Action<TBindable> uiSetter, Action<TEvent> propertySetter) {
        this.uiSetter = uiSetter;
        this.propertySetter = propertySetter;
    }

    private void OnPropertyChanged(TBindable value) {
        if (locked)
            return;

        locked = true;
        uiSetter(value);
        locked = false;
    }

    private void OnUIChanged(TEvent value) {
        if (locked)
            return;

        locked = true;
        propertySetter(value);
        locked = false;
    }

    public static void Create(UnityEvent<TEvent> uiEvent, Bindable<TBindable> property, Action<TBindable> uiSetter, Action<TEvent> propertySetter) {
        var binding = new UIBinding<TEvent, TBindable>(uiSetter, propertySetter);
            
        property.BindAndInvoke(binding.OnPropertyChanged);
        uiEvent.AddListener(binding.OnUIChanged);
    }
}