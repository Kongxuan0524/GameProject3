using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using LVL;
using ACT;
using BVT;
using BIE;
using TAS;
using PLT;
using CUT;

public class GTWorld : GTSingleton<GTWorld>
{
    public EffectSystem   Ect     { get; private set; }
    public FlywordSystem  Fly     { get; private set; }
    public HUDSystem      Hud     { get; private set; }
    public ActSystem      Act     { get; private set; }
    public NodeTreeSystem Nts     { get; private set; }
    public LevelSystem    Lvl     { get; private set; }
    public GuideSystem    Bie     { get; private set; }
    public TaskSystem     Tas     { get; private set; }
    public PlotSystem     Plt     { get; private set; }
    public VideoSystem    Cut     { get; private set; }

    public void EnterWorld(int mapID)
    {
        Lvl = GameObject.FindObjectOfType<LevelSystem>();
        if (Lvl == null)
        {
            GameObject go = new GameObject("LevelSystem");
            Lvl = go.AddComponent<LevelSystem>();
        }
        else
        {
            Lvl.DelAllElements();
        }
        CharacterManager.Instance.SetRoot(Lvl.transform);
        Ect = new EffectSystem();
        Fly = new FlywordSystem();
        Hud = new HUDSystem();
        Act = new ActSystem();
        Nts = new NodeTreeSystem();
        Plt = new PlotSystem();
        Cut = new VideoSystem();
        Lvl.Id = mapID;
        Lvl.Startup();
    }

    public void EnterGuide()
    {
        if (Bie == null)
        {
            Bie = new GuideSystem();
            Bie.Startup();
        }
    }

    public void ResetGuide()
    {
        if (Bie != null)
        {
            Bie.ResetCurGuide();
        }
    }

    public void Release()
    {
        if (Act != null)
        {
            Act.Release();
        }
        if (Hud != null)
        {
            Hud.Release();
        }
        if (Fly != null)
        {
            Fly.Release();
        }
        if (Ect != null)
        {
            Ect.Release();
        }
        if (Nts != null)
        {
            Nts.Release();
        }
        if (Lvl != null)
        {
            Lvl.Release();
        }
    }

    public void Execute()
    {
        if (Act != null)
        {
            Act.Execute();
        }
        if (Hud != null)
        {
            Hud.Execute();
        }
        if (Fly != null)
        {
            Fly.Execute();
        }
        if (Nts != null)
        {
            Nts.Execute();
        }
        if (Bie != null)
        {
            Bie.Execute();
        }
    }
}