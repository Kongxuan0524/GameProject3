using UnityEngine;
using System.Collections;
using System;
using System.Xml;

namespace BIE
{
    public class GuidePlotTask : GuideBase
    {
        public Int16 PlotID;

        public override void Enter()
        {
            base.Enter();
            GTWorld.Instance.Plt.Trigger(PlotID, Stop, Finish);
        }

        public override void Stop()
        {
            base.Stop();
            this.State = EGuideState.TYPE_FINISH;
        }

        public override void Finish()
        {
            base.Finish();
        }

        public override void Read(XmlElement os)
        {
            base.Read(os);
            this.PlotID = os.GetInt16("PlotID");
        }
    }
}
