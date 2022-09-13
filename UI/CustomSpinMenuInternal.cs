using UnityEngine;

namespace SpinCore.UI; 

internal sealed class CustomSpinMenuInternal : SpinMenu {
    public override Vector2 MenuTransitionAnchorOffset => new(1.5f, 0.0f);
}