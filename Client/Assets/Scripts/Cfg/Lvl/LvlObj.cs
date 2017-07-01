using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Xml;

namespace CFG
{
    public class LvlObj : LvlElement
    {
        public ELvlObjType       Type    = ELvlObjType.Build;
        public Vector3           Pos     = Vector3.zero;
        public Vector3           Euler   = Vector3.zero;
        public Vector3           Scale   = Vector3.one;

        public override void Read(XmlElement os)
        {
            this.Id     = os.GetInt32("Id");
            this.Type   = (ELvlObjType)os.GetInt32("Type");
            this.Pos    = os.GetVector3("Name");
            this.Euler  = os.GetVector3("Path");
            this.Scale  = os.GetVector3("Scale");
        }

        public override void Write(XmlDocument doc, XmlElement os)
        {
            DCFG.Write(doc, os, "Id",     this.Id);
            DCFG.Write(doc, os, "Type",   (int)this.Type);
            DCFG.Write(doc, os, "Pos",    this.Pos);
            DCFG.Write(doc, os, "Euler",  this.Euler);
            DCFG.Write(doc, os, "Scale",  this.Scale);
        }
    }
}

