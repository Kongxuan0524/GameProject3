﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public sealed class SceneLoading : IScene
{ 
    public float              mWaitTime = 0.1f;
    public float              mWaitTimer = 0f;
    public bool               mLoading = false;
    public int                mLoadingSceneId;
    public UILoading          mLoadingWindow;
    public AsyncOperation     mAsync;

    public override void Enter()
    {
        base.Enter();
        CommandLoadScene ev = Cmd as CommandLoadScene;
        if (GTWorld.Instance.Bie != null)
        {
            GTWorld.Instance.Bie.PauseGuide = true;
        }
        GTWindowManager. Instance.Release();
        GTWorld.         Instance.Release();
        GTPoolManager.   Instance.Release();
        GTWindowManager. Instance.OpenWindow(EWindowID.UILoading);

        mLoadingSceneId = ev.SceneID;
        mLoadingWindow  = (UILoading)GTWindowManager.Instance.GetWindow(EWindowID.UILoading);
        mWaitTime       = Time.realtimeSinceStartup;
    }

    public override void Execute()
    {
        if (mLoading == false)
        {
            if (Time.realtimeSinceStartup - mWaitTime > 0.1f)
            {
                GTCoroutinueManager.Instance.StartCoroutine(DoLoadScene());
                mLoading = true;
            }
        }

        if (mAsync != null)
        {
            if (mLoadingWindow.CacheTransform != null)
            {
                mLoadingWindow.UpdateProgress(mAsync.progress);
            }
            if (mAsync.isDone)
            {
                mLoading = false;
                mAsync = null;
                OnSceneWasLoaded();
            }
        }    
    }

    public override void Exit()
    {
        mWaitTimer = 0;
        mLoading = false;
        mAsync = null;
        GTCoroutinueManager.Instance.StartCoroutine(DoFadeScene());
    }

    void OnSceneWasLoaded()
    {
        DScene db = ReadCfgScene.GetDataById(mLoadingSceneId);
        GTAudioManager.Instance.PlayMusic(db.SceneMusic);
        GTLauncher.    Instance.ChangeState(GTLauncher.Instance.NextSceneType,Cmd);
        GTWorld.       Instance.ResetGuide();
        GTWorld.       Instance.EnterWorld(mLoadingSceneId);
    }

    void OnSceneWasFadeOut()
    {
        GTGlobal.CurSceneID = mLoadingSceneId;
        GTWindowManager.Instance.CloseWindow(EWindowID.UILoading);
        if (GTWorld.Instance.Bie != null)
        {
            GTWorld.Instance.Bie.PauseGuide = false;
        }
    }

    IEnumerator DoLoadScene()
    {
        if (mAsync == null)
        {
            mAsync = GTLauncher.Instance.LoadLevelById(mLoadingSceneId);
        }
        mAsync.allowSceneActivation = false;
        while (mAsync.progress < 0.9f)
        {
            yield return new WaitForEndOfFrame();
        }
        mAsync.allowSceneActivation = true;
    }

    IEnumerator DoFadeScene()
    {
        yield return new WaitForSeconds(1f);
        OnSceneWasFadeOut();
    }
}
