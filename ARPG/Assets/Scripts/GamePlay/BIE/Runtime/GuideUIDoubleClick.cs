using UnityEngine;
using System.Collections;
using System.Xml;

namespace BIE
{
    public class GuideUIDoubleClick : GuideUIEvent
    {
        public override void Enter()
        {
            base.Enter();
            this.GuideWindow.DoGuideForDoubleClick(Manager.GetData(BtnName), this);
        }

        public override void Finish()
        {
            base.Finish();
            GTWindowManager.Instance.CloseWindow(EWindowID.UIGuide);
        }

        public override void Stop()
        {
            base.Stop();
            GTWindowManager.Instance.CloseWindow(EWindowID.UIGuide);
        }
    }
}

