using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System;

namespace CFG
{
    public class LvlBarrier : LvlElement
    {
        public float        Width = 7;
        public Vector3      Pos   = Vector3.zero;
        public Vector3      Euler = Vector3.zero;

        public override void Read(XmlElement os)
        {
            this.Id      = os.GetInt32("Id");
            this.Width   = os.GetFloat("Width");
            this.Pos     = os.GetVector3("Pos");
            this.Euler   = os.GetVector3("Euler");
        }

        public override void Write(XmlDocument doc, XmlElement os)
        {
            DCFG.Write(doc, os, "Id",      this.Id);
            DCFG.Write(doc, os, "Width",   this.Width);
            DCFG.Write(doc, os, "Pos",     this.Pos);
            DCFG.Write(doc, os, "Euler",   this.Euler);
        }
    }
}

