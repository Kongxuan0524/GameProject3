using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public sealed class SceneCity : IScene
{
    public override IEnumerator OpenWindows()
    {
        GTWindowManager.Instance.OpenWindow(EWindowID.UI_HOME);
        yield return null;
    }
}
