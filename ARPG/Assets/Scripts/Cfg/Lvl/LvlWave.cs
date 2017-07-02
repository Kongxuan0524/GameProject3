using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace CFG
{
    [System.Serializable]
    public class LvlWave : LvlElement
    {
        public string                    IndexName = string.Empty;
        public float                     Delay;
        public ELvlMonsterWaveSpawn      Spawn;
        public int                       AddBuffID;
        public List<LvlMonster>          Monsters = new List<LvlMonster>();


        public override void Read(XmlElement os)
        {
            this.Id              = os.GetInt32("Id");
            this.IndexName       = os.GetString("IndexName");
            this.Delay           = os.GetFloat("Delay");
            this.Spawn           = (ELvlMonsterWaveSpawn)os.GetInt32("Spawn");
            this.AddBuffID       = os.GetInt32("AddBuffID");
            this.Monsters        = ReadListFromChildAttribute<LvlMonster>(os, "Monsters");
        }

        public override void Write(XmlDocument doc, XmlElement os)
        {
            DCFG.Write(doc, os, "Id",           Id);
            DCFG.Write(doc, os, "IndexName",    IndexName);
            DCFG.Write(doc, os, "Delay",        Delay);
            DCFG.Write(doc, os, "Spawn",   (int)Spawn);
            DCFG.Write(doc, os, "AddBuffID",    AddBuffID);
            DCFG.Write(doc, os, "Monsters",     Monsters);
        }
    }
}

