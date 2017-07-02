using UnityEngine;
using System.Collections;
using System;
using System.Xml;

public class DMachine : DObj<int>
{
    public const int TYPE_NONE      = 0;
    public const int TYPE_MOVE      = 1;
    public const int TYPE_PRICK     = 2;
    public const int TYPE_ANIMATION = 3;
    public const int TYPE_TRIGGER   = 4;
    public const int TYPE_COLLIDER  = 5;
    public const int TYPE_DROPOUT   = 6;

    public int    Id;
    public string Path = string.Empty;
    public string Name = string.Empty;
    public int    Type;
    public int    NavMeshLayer;
    
    public override int GetKey()
    {
        return Id;
    }

    public override void Read(XmlElement element)
    {
        this.Id           = element.GetInt32("Id");
        this.Name         = element.GetString("Name");
        this.Path         = element.GetString("Path");
        this.Type         = element.GetInt32("Type");
        this.NavMeshLayer = element.GetInt32("NavMeshLayer");
    }
}

public class ReadCfgMachine : DReadBase<int, DMachine>
{

}