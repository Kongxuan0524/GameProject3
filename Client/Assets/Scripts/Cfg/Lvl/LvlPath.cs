using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace CFG
{
    public class LvlPath : LvlElement
    {
        public ELvlPathType         Type      = ELvlPathType.Linear;
        public List<LvlPathNode>    PathNodes = new List<LvlPathNode>();

        public override void Read(XmlElement os)
        {
            this.Id         = os.GetInt32("Id");
            this.Type       = (ELvlPathType)os.GetInt32("Type");
            this.PathNodes  = ReadListFromChildAttribute<LvlPathNode>(os, "PathNodes");
        }

        public override void Write(XmlDocument doc, XmlElement os)
        {
            DCFG.Write(doc, os, "Id",        this.Id);
            DCFG.Write(doc, os, "Type",      this.Type.ToString());
            DCFG.Write(doc, os, "PathNodes", this.PathNodes);
        }
    }
}

