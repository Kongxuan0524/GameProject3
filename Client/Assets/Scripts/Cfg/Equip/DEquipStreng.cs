using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;

public class DEquipStreng : DObj<int>
{
    public int                    Id;
    public List<KStrengthValue>   Propertys = new List<KStrengthValue>();

    public override int GetKey()
    {
        return Id;
    }

    public override void Read(XmlElement element)
    {
        this.Id = element.GetInt32("Id");
        for (int i = 1; i <= 6; i++)
        {
            KStrengthValue data = new KStrengthValue();
            data.Property    = (EAttr)element.GetInt32("PropertyId" + i);
            data.Value       = element.GetInt32("PropertyNum" + i);
            data.UnlockLevel = element.GetInt32("PropertyLevel" + i);
            this.Propertys.Add(data);
        }
    }
}


public class ReadCfgEquipStreng : DReadBase<int, DEquipStreng>
{

}