using UnityEngine;
using System.Collections;
using System.Xml;

namespace CFG
{
    public class LvlMachine : LvlElement
    {
        public Vector3 Pos;
        public Vector3 Euler;

        public override void Read(XmlElement os)
        {
            this.Id    = os.GetInt32("Id");
            this.Pos   = os.GetVector3("Pos");
            this.Euler = os.GetVector3("Euler");
        }

        public override void Write(XmlDocument doc, XmlElement os)
        {
            DCFG.Write(doc, os, "Id",    Id);
            DCFG.Write(doc, os, "Pos",   Pos);
            DCFG.Write(doc, os, "Euler", Euler);
        }
    }
}

