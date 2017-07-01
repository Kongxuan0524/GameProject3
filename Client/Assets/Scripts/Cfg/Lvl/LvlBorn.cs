using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Xml;

namespace CFG
{
    public class LvlBorn : LvlElement
    {
        public EBattleCamp    Camp;
        public Vector3        Pos    = Vector3.zero;
        public Vector3        Euler  = Vector3.zero;

        public override void Read(XmlElement os)
        {
            this.Camp       = os.GetEnum<EBattleCamp>("Camp");
            this.Pos        = os.GetVector3("Pos");
            this.Euler      = os.GetVector3("Euler");
        }

        public override void Write(XmlDocument doc, XmlElement os)
        {
            DCFG.Write(doc,os,  "Camp", (int)this.Camp);
            DCFG.Write(doc, os, "Pos",            Pos);
            DCFG.Write(doc, os, "Euler",          Euler);
        }
    }
}

