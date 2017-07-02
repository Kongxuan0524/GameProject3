using UnityEngine;
using System.Collections;
using System.Xml;

namespace BIE
{
    public class GuideUIEvent : GuideBase
    {
        public string BtnName = "BtnName";

        public override bool Check()
        {
            if (Manager.GetData(BtnName) == null)
            {
                return false;
            }
            return base.Check();
        }

        public override void Read(XmlElement os)
        {
            base.Read(os);
            this.BtnName = os.GetString("BtnName");
        }
    }
}

