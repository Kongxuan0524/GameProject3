using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public sealed class ScenePVE : IScene
{
    public override IEnumerator OpenWindows()
    {
        GTWindowManager.Instance.OpenWindow(EWindowID.UIHome);
        yield return null;
    }
}
