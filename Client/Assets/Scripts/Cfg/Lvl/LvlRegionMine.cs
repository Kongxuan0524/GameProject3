using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace CFG
{
    public class LvlRegionMine : LvlElement
    {
        public int           RegionID;
        public int           MineID;
        public int           MaxCount;
        public float         RebornCD;

        public override void Read(XmlElement os)
        {
            this.Id            = os.GetInt32("Id");
            this.RegionID      = os.GetInt32("RegionID");
            this.MineID        = os.GetInt32("MineID");
            this.MaxCount      = os.GetInt32("MaxCount");
            this.RebornCD      = os.GetFloat("RebornCD");
        }

        public override void Write(XmlDocument doc, XmlElement os)
        {
            DCFG.Write(doc, os, "Id",       Id);
            DCFG.Write(doc, os, "RegionID", RegionID);
            DCFG.Write(doc, os, "MineID",   MineID);
            DCFG.Write(doc, os, "MaxCount", MaxCount);
            DCFG.Write(doc, os, "RebornCD", RebornCD);
        }
    }

}
