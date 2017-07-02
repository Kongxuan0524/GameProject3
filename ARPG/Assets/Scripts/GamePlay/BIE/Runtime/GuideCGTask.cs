using UnityEngine;
using System.Collections;
using System;
using System.Xml;

namespace BIE
{
    public class GuideCGTask : GuideBase
    {
        public Int16 VideoID = 1;

        public override void Enter()
        {
            base.Enter();
            GTWorld.Instance.Cut.Trigger(VideoID, 
            (videoPlayer)=>
            {
                Stop();
            },
            (videoPlayer) => 
            {
                Finish();
            });
        }

        public override void Stop()
        {
            base.Stop();
        }

        public override void Finish()
        {
            base.Finish();
        }

        public override void Read(XmlElement os)
        {
            base.Read(os);
            this.VideoID = os.GetInt16("VideoID");
        }
    }
}