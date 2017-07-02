using UnityEngine;
using System.Collections;
using System.Xml;

namespace BIE
{
    public class GuideTimeLimitTask : GuideBase
    {
        public int Seconds = 10;

        private float m_SecondTimer = 0;

        public override void Enter()
        {
            base.Enter();
            m_SecondTimer = Time.realtimeSinceStartup;
        }

        public override void Execute()
        {
            base.Execute();
            if (Time.realtimeSinceStartup - m_SecondTimer > Seconds)
            {
                this.Finish();
            }
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
            this.Seconds = os.GetInt32("Seconds");
        }

        public override void Write(XmlDocument doc, XmlElement os)
        {
            base.Write(doc, os);
        }
    }
}

