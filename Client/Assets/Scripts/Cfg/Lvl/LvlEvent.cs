using UnityEngine;
using System.Collections;
using System;
using System.Xml;

namespace CFG
{
    [System.Serializable]
    public class LvlEvent : LvlElement
    {
        public ELvlTrigger          Type             = ELvlTrigger.TYPE_WAVESET;
        public bool                 Show             = true;
        public ELvlTriggerCondition TriggerCondition = ELvlTriggerCondition.TYPE_AWAKE_REGION;

        public override void Read(XmlElement os)
        {
            this.Id               = os.GetInt32("Id");
            this.Type             = (ELvlTrigger)os.GetInt32("Type");
            this.Show             = os.GetBool("Show");
            this.TriggerCondition = (ELvlTriggerCondition)os.GetInt32("TriggerCondition");
        }

        public override void Write(XmlDocument doc, XmlElement os)
        {
            DCFG.Write(doc, os, "Id",                    this.Id);
            DCFG.Write(doc, os, "Type",             (int)this.Type);
            DCFG.Write(doc, os, "Show",                  this.Show);
            DCFG.Write(doc, os, "TriggerCondition", (int)this.TriggerCondition);
        }
    }
}

