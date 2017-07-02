using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

namespace CFG
{
    public class LvlConfig : DCFG
    {
        public int                   Id;
        public float                 Delay;
        public string                LvlName         = string.Empty;
        public bool                  AllowRide       = true;
        public bool                  AllowPK         = true;
        public bool                  AllowTrade      = true;
        public bool                  AllowFight      = true;
        public Vector3               CameraPos       = Vector3.zero;
        public Vector3               CameraEuler     = Vector3.zero;

        public List<LvlBorn>          Borns           = new List<LvlBorn>();
        public List<LvlBarrier>       Barriers        = new List<LvlBarrier>();
        public List<LvlPortal>        Portals         = new List<LvlPortal>();
        public List<LvlRegion>        Regions         = new List<LvlRegion>();
        public List<LvlRegionMonster> RegionMonsters  = new List<LvlRegionMonster>();
        public List<LvlRegionMine>    RegionMines     = new List<LvlRegionMine>();
        public List<LvlRegionWave>    RegionWaves     = new List<LvlRegionWave>();
        public List<LvlNpc>           Npcs            = new List<LvlNpc>();
        public List<LvlObj>           Objs            = new List<LvlObj>();
        public List<LvlMonster>       Monsters        = new List<LvlMonster>();
        public List<LvlPath>          Paths           = new List<LvlPath>();
        public List<LvlMine>          Mines           = new List<LvlMine>();

        public override void Read(XmlElement os)
        {
            this.Id             = os.GetInt32("Id");
            this.Delay          = os.GetFloat("Delay");
            this.LvlName        = os.GetString("LvlName");
            this.AllowRide      = os.GetBool("AllowRide");
            this.AllowPK        = os.GetBool("AllowPK");
            this.AllowTrade     = os.GetBool("AllowTrade");
            this.AllowFight     = os.GetBool("AllowFight");
            this.CameraPos      = os.GetVector3("CameraPos");
            this.CameraEuler    = os.GetVector3("CameraEuler");

            foreach (var current in GetChilds(os))
            {
                switch(current.Name)
                {
                    case "Borns":
                        this.Borns         = ReadList<LvlBorn>(current);
                        this.A             = this.Borns.Find((item) => { return item.Camp == EBattleCamp.A; });
                        this.B             = this.Borns.Find((item) => { return item.Camp == EBattleCamp.B; });
                        this.C             = this.Borns.Find((item) => { return item.Camp == EBattleCamp.C; });
                        break;
                    case "Barriers":
                        this.Barriers      = ReadList<LvlBarrier>(current);
                        break;
                    case "Portals":
                        this.Portals       = ReadList<LvlPortal>(current);
                        break;
                    case "Regions":
                        this.Regions       = ReadList<LvlRegion>(current);
                        break;
                    case "RegionMonsters":
                        this.RegionMonsters = ReadList<LvlRegionMonster>(current);
                        break;
                    case "RegionMines":
                        this.RegionMines   = ReadList<LvlRegionMine>(current);
                        break;
                    case "RegionWaves":
                        this.RegionWaves   = ReadList<LvlRegionWave>(current);
                        break;
                    case "Npcs":
                        this.Npcs          = ReadList<LvlNpc>(current);
                        break;
                    case "Objs":
                        this.Objs          = ReadList<LvlObj>(current);
                        break;
                    case "Monsters":
                        this.Monsters      = ReadList<LvlMonster>(current);
                        break;
                    case "Paths":
                        this.Paths         = ReadList<LvlPath>(current);
                        break;
                    case "Mines":
                        this.Mines         = ReadList<LvlMine>(current);
                        break;
                }
            }
        }

        public override void Write(XmlDocument doc, XmlElement os)
        {
            DCFG.Write(doc, os, "Id",            Id);
            DCFG.Write(doc, os, "Delay",         Delay);
            DCFG.Write(doc, os, "LvlName",       LvlName);
            DCFG.Write(doc, os, "AllowRide",     AllowRide);
            DCFG.Write(doc, os, "AllowPK",       AllowPK);
            DCFG.Write(doc, os, "AllowTrade",    AllowTrade);
            DCFG.Write(doc, os, "AllowFight",    AllowFight);
            DCFG.Write(doc, os, "Borns",         Borns);
            DCFG.Write(doc, os, "Barriers",      Barriers);
            DCFG.Write(doc, os, "Portals",       Portals);
            DCFG.Write(doc, os, "Regions",       Regions);
            DCFG.Write(doc, os, "RegionMonsters",RegionMonsters);
            DCFG.Write(doc, os, "RegionMines",   RegionMines);
            DCFG.Write(doc, os, "RegionWaves",   RegionWaves);
            DCFG.Write(doc, os, "Npcs",          Npcs);
            DCFG.Write(doc, os, "Objs",          Objs);
            DCFG.Write(doc, os, "Monsters",      Monsters);
            DCFG.Write(doc, os, "Paths",         Paths);
            DCFG.Write(doc, os, "Mines",         Mines);
        }

        public LvlBorn A { get; private set; }
        public LvlBorn B { get; private set; }
        public LvlBorn C { get; private set; }
    }
}

