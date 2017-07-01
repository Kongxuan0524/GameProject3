using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace CFG
{
    public class LvlRegion : LvlElement
    {
        public string                    Name         = string.Empty;
        public Vector3                   Pos          = Vector3.zero;
        public Vector3                   Euler        = Vector3.zero;
        public Vector3                   Scale        = Vector3.one;
        public bool                      AwakeActive  = false;
        public List<LvlEvent>            Events       = new List<LvlEvent>();

        public override void Read(XmlElement os)
        {
            this.Id            = os.GetInt32("Id");
            this.Name          = os.GetString("Name");
            this.Pos           = os.GetVector3("Pos");
            this.Euler         = os.GetVector3("Euler ");
            this.Scale         = os.GetVector3("Scale");
            this.AwakeActive   = os.GetBool("AwakeActive");
            this.Events        = ReadListFromChildAttribute<LvlEvent>(os, "Events");
        }

        public override void Write(XmlDocument doc, XmlElement os)
        {
            DCFG.Write(doc, os, "Id",             this.Id);
            DCFG.Write(doc, os, "Name",           this.Name);
            DCFG.Write(doc, os, "Pos",            this.Pos);
            DCFG.Write(doc, os, "Euler",          this.Euler);
            DCFG.Write(doc, os, "Scale",          this.Scale);
            DCFG.Write(doc, os, "AwakeActive",    this.AwakeActive);
            DCFG.Write(doc, os, "Events",         this.Events);
        }
    }
}

