using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml;

public class DRelics : DObj<int>
{
    public int            Id;
    public string         Name;
    public string         Icon;
    public string         Desc;
    public string         Model;
    public int            ActiveEffectID;
    public int            DitiveEffectID;
    public int            AddProperty;
    public int[]          LevelRequireExp = new int[5];
    public int[]          ArtificeCostID = new int[3];
    public EAttr[]        PropertyID = new EAttr[3];
    public int[]          PropertyNum = new int[3];
    public int            SkillID;
    public Vector3        StagePos;
    public Vector3        StageEuler;
    public float          StageScale;

    public override int GetKey()
    {
        return Id;
    }

    public override void Read(XmlElement element)
    {
        this.Id             = element.GetInt32("Id");
        this.Name           = element.GetString("Name");
        this.Icon           = element.GetString("Icon");
        for (int i = 1; i <= 3; i++)
        {
            EAttr p = (EAttr)element.GetInt32("PropertyID" + i);
            int num = element.GetInt32("PropertyNum" + i);
            this.PropertyID[i - 1] = p;
            this.PropertyNum[i - 1] = num;
        }

        this.Model          = element.GetString("Model");
        this.ActiveEffectID = element.GetInt32("ActiveEffectID");
        this.DitiveEffectID = element.GetInt32("DitiveEffectID");

        for (int i = 1; i <= 5; i++)
        {
            int exp = element.GetInt32("LevelExp" + i);
            this.LevelRequireExp[i - 1] = exp;
        }
        for (int i = 1; i <= 3; i++)
        {
            int id = element.GetInt32("ArtificeCostID" + i);
            this.ArtificeCostID[i - 1] = id;
        }
        this.Desc           = element.GetString("Desc");
        this.StagePos       = element.GetString("StagePos").ToVector3(true);
        this.StageEuler     = element.GetString("StageEuler").ToVector3(true);
        this.StageScale     = element.GetFloat("StageScale");
        this.SkillID        = element.GetInt32("SkillID");
        this.AddProperty    = element.GetInt32("AddProperty");
    }
}

public class ReadCfgRelics : DReadBase<int, DRelics>
{

}