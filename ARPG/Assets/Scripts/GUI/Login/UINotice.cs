﻿using UnityEngine;
using System.Collections;
using System;

public class UINotice : GTWindow
{
    public UINotice()
    {
        mResident = false;
        mResPath = "Login/UINotice";
        Type = EWindowType.DIALOG;
    }

    private GameObject btnClose;


    protected override void OnAwake()
    {
        btnClose = transform.Find("Pivot/Btn_Close").gameObject;
    }

    protected override void OnAddButtonListener()
    {
        UIEventListener.Get(btnClose).onClick = OnCloseClick;
    }

    protected override void OnAddHandler()
    {

    }

    protected override void OnClose()
    {
        
    }

    protected override void OnDelHandler()
    {
        
    }

    protected override void OnEnable()
    {
        
    }

    private void OnCloseClick(GameObject go)
    {
        Hide();
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_BACK);
    }
}
