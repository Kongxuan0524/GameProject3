using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace CFG
{
    public class LvlRegionWave : LvlElement
    {
        public List<LvlWave>        Waves  = new List<LvlWave>();
        public List<LvlEvent>       Events = new List<LvlEvent>();

        public override void Read(XmlElement os)
        {
            this.Id         = os.GetInt32("Id");
            this.Waves      = ReadListFromChildAttribute<LvlWave>(os, "Waves");
            this.Events     = ReadListFromChildAttribute<LvlEvent>(os, "Events");
        }

        public override void Write(XmlDocument doc, XmlElement os)
        {
            DCFG.Write(doc, os, "Id",        this.Id);
            DCFG.Write(doc, os, "Waves",     this.Waves);
            DCFG.Write(doc, os, "Events",    this.Events);
        }
    }
}

