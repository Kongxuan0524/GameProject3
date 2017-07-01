using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System;

namespace CFG
{
    public class LvlMine : LvlElement
    {
        public Vector3 Pos;
        public Vector3 Euler;
        public float   RebornCD;
        public int     DropItemCount;

        public override void Read(XmlElement os)
        {
            this.Id            = os.GetInt32("Id");
            this.DropItemCount = os.GetInt32("DropItemCount");
            this.RebornCD      = os.GetInt32("RebornCD");
            this.Pos           = os.GetVector3("Pos");
            this.Euler         = os.GetVector3("Euler");
        }

        public override void Write(XmlDocument doc, XmlElement os)
        {
            DCFG.Write(doc, os, "Id",             this.Id);
            DCFG.Write(doc, os, "DropItemCount",  this.DropItemCount);
            DCFG.Write(doc, os, "RebornCD",       this.RebornCD);
            DCFG.Write(doc, os, "Pos",            this.Pos);
            DCFG.Write(doc, os, "Euler",          this.Euler);
        }
    }
}

