﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml;

public class DActor : DObj<int>
{
    public int               Id;
    public int               Level;
    public int               Title;
    public string            Name;
    public string            Desc;
    public string            Icon;
    public EActorRace        Race;
    public EActorSex         Sex;
    public EActorType        Type;
    public EActorSort        Sort;
    public EActorUnit        Unit;
    public ECarrer           Carrer;
    public int               Group;
    public int               Quality;
    public int               Exp;
    public int               Gold;
    public float             WSpeed;
    public float             RSpeed;
    public int               BornEffectID;
    public int               DeadEffectID;
    public int               Model;
    public Vector3           StagePos;
    public float             StageScale;
    public string            Skill;
    public Dictionary<EAttr, int>         Propertys = new Dictionary<EAttr, int>();
    public Dictionary<EActorNature, bool> Natures   = new Dictionary<EActorNature, bool>();

    public override int GetKey()
    {
        return Id;
    }

    public override void Read(XmlElement element)
    {
        this.Id           = element.GetInt32("Id");
        this.Level        = element.GetInt32("Level");
        this.Title        = element.GetInt32("Title");
        this.Name         = element.GetString("Name");
        this.Desc         = element.GetString("Desc");
        this.Icon         = element.GetString("Icon");

        this.Race         = (EActorRace)element.GetInt32("Race");
        this.Type         = (EActorType)element.GetInt32("Type");
        this.Sex          = (EActorSex)element.GetInt32("Sex");
        this.Sort         = (EActorSort)element.GetInt32("Sort");
        this.Unit         = (EActorUnit)element.GetInt32("Unit");
        this.Carrer       = (ECarrer)element.GetInt32("Carrer");
        this.Group        = element.GetInt32("Group");
        this.Quality      = element.GetInt32("Quality");

        this.RSpeed       = element.GetFloat("RSpeed");
        this.WSpeed       = element.GetFloat("WSpeed");
        this.BornEffectID = element.GetInt32("BornEffectID");
        this.DeadEffectID = element.GetInt32("DeadEffectID");

        this.StagePos     = element.GetString("StagePos").ToVector3(true);
        this.StageScale   = element.GetFloat("StageScale");
        this.Skill        = element.GetString("Skill");
        this.Model        = element.GetInt32("Model");
        this.Exp          = element.GetInt32("Exp");
        this.Gold         = element.GetInt32("Gold");

        for (int i = 1; i <= 16; i++)
        {
            int v = element.GetInt32("P" + i.ToString());
            this.Propertys.Add((EAttr)i, v);
        }

        this.Natures[EActorNature.CAN_MOVE]         = element.GetInt32("CanMove") == 1;
        this.Natures[EActorNature.CAN_KILL]         = element.GetInt32("CanKill") == 1;
        this.Natures[EActorNature.CAN_MANUALATTACK] = element.GetInt32("CanManualAttack") == 1;
        this.Natures[EActorNature.CAN_TURN]         = element.GetInt32("CanTurn") == 1;
        this.Natures[EActorNature.CAN_STUN]         = element.GetInt32("CanStun") == 1;
        this.Natures[EActorNature.CAN_BEATBACK]     = element.GetInt32("CanBeatBack") == 1;
        this.Natures[EActorNature.CAN_BEATFLY]      = element.GetInt32("CanBeatFly") == 1;
        this.Natures[EActorNature.CAN_BEATDOWN]     = element.GetInt32("CanBeatDown") == 1;
        this.Natures[EActorNature.CAN_WOUND]        = element.GetInt32("CanWound") == 1;
        this.Natures[EActorNature.CAN_REDUCESPEED]  = element.GetInt32("CanReduceSpeed") == 1;
        this.Natures[EActorNature.CAN_FIXBODY]      = element.GetInt32("CanFixBody") == 1;
        this.Natures[EActorNature.CAN_SLEEP]        = element.GetInt32("CanSleep") == 1;
        this.Natures[EActorNature.CAN_VARISTION]    = element.GetInt32("CanVaristion") == 1;
        this.Natures[EActorNature.CAN_PARALY]       = element.GetInt32("CanParaly") == 1;
        this.Natures[EActorNature.CAN_FEAR]         = element.GetInt32("CanFear") == 1;
    }
}


public class ReadCfgActor : DReadBase<int, DActor>
{
  
}