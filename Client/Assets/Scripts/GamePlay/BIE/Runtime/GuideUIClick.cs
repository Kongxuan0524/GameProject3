using UnityEngine;
using System.Collections;
using System.Xml;

namespace BIE
{
    public class GuideUIClick : GuideUIEvent
    {
        public override void Enter()
        {
            base.Enter();
            this.GuideWindow.DoGuideForClick(Manager.GetData(BtnName), this);
        }

        public override void Finish()
        {
            base.Finish();
            GTWindowManager.Instance.CloseWindow(EWindowID.UI_GUIDE);
        }

        public override void Stop()
        {
            base.Stop();
            GTWindowManager.Instance.CloseWindow(EWindowID.UI_GUIDE);
        }
    }
}
