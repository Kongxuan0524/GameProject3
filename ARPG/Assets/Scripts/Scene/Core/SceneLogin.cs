using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public sealed class SceneLogin : IScene
{
    public override IEnumerator OpenWindows()
    {
        GTWindowManager.Instance.OpenWindow(EWindowID.UILogin);
        yield return null;
    }
}
