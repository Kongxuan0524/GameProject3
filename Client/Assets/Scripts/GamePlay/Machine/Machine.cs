using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : IActor
{
    public GameObject           Obj                       { get; set; }
    public Transform            ObjTrans                  { get; set; }
    public Transform            CacheTransform            { get; set; }

    public int                  GUID                      { get; set; }
    public int                  ID                        { get; set; }

    public Vector3              Dir
    {
        get { return CacheTransform.forward; }
        set { CacheTransform.forward = value; }
    }

    public Vector3              Euler
    {
        get { return CacheTransform.eulerAngles; }
        set { CacheTransform.eulerAngles = value; }
    }

    public Vector3              Pos
    {
        get { return CacheTransform.position; }
        set { CacheTransform.position = value; }
    }

    public Vector3              Scale
    {
        get { return CacheTransform.localScale; }
        set { CacheTransform.localScale = value; }
    }

    public void Load(KTransform bornData)
    {
        
    }

    public void Destroy()
    {
        
    }

    public void Execute()
    {
        
    }

    public void Release()
    {
        
    }

    public void BecameVisable()
    {
        
    }

    public void BecameInVisable()
    {
        
    }

    public void Pause(bool pause)
    {
        
    }

    public bool IsDead()
    {
        return false;
    }

    public bool IsDestroy()
    {
        return false;
    }
}
