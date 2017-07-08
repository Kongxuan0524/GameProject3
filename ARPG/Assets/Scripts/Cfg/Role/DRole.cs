using UnityEngine;
using System.Collections;
using System;
using System.Xml;

public class DRole : DObj<int>
{
    public int    Id;
    public string CarrerName = string.Empty;
    public int    Star1;
    public int    Star2;
    public int    Star3;
    public int    Star4;
    public int    DisplayWeapon;

    public override int GetKey()
    {
        return Id;
    }

    public override void Read(XmlElement element)
    {
        this.Id            = element.GetInt32("Id");
        this.CarrerName    = element.GetString("CarrerName");
        this.Star1         = element.GetInt32("Star1");
        this.Star2         = element.GetInt32("Star2");
        this.Star3         = element.GetInt32("Star3");
        this.Star4         = element.GetInt32("Star4");
        this.DisplayWeapon = element.GetInt32("DisplayWeapon");
    }
}

public class ReadCfgRole : DReadBase<int, DRole>
{

}