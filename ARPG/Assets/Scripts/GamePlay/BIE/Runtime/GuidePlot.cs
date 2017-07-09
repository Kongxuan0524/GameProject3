using UnityEngine;
using System.Collections;
using System;
using System.Xml;

namespace BIE
{
    public class GuidePlot : GuideBase
    {
        public Int16 PlotID;

        public override void Enter()
        {
            base.Enter();
            GTWorld.Instance.Plt.Trigger(PlotID, Stop, Finish);
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

        public override void Write(XmlDocument doc, XmlElement os)
        {
            base.Write(doc, os);
            DCFG.Write(doc, os, "PlotID", PlotID);
        }
    }
}
