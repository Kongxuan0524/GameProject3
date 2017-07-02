﻿using UnityEngine;
using System.Collections;

public interface IEntiny : IObj
{
    GameObject Obj            { get; set; }
    Transform  CacheTransform { get; set; }

    void       Load(KTransform bornData);
    void       Destroy();
    void       Execute();
    void       Release();
    void       BecameVisable();
    void       BecameInVisable();
    void       Pause(bool pause);
    bool       IsDead();
    bool       IsDestroy();
}
