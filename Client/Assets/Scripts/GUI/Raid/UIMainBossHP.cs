﻿using UnityEngine;
using System.Collections;
using System;

public class UIMainBossHP : GTWindow
{
    private UISlider m_HPSlider;
    private UISprite m_HPHeadIcon;

    public UIMainBossHP()
    {
        Type = EWindowType.FIXED;
        mResPath = "Raid/UIMainBossHP";
    }

    protected override void OnAwake()
    {
        Transform pivot = transform.FindChild("Pivot");
        m_HPSlider   = pivot.FindChild("HPSlider").GetComponent<UISlider>();
        m_HPHeadIcon = pivot.FindChild("HPHead/Icon").GetComponent<UISprite>();
    }

    protected override void OnEnable()
    {
        m_HPSlider.value = 1;
    }

    protected override void OnAddButtonListener()
    {
        
    }

    protected override void OnAddHandler()
    {
        GTEventCenter.AddHandler(GTEventID.TYPE_UP_BOSS_HP, ShowBossHP);
    }

    protected override void OnClose()
    {

    }

    protected override void OnDelHandler()
    {
        GTEventCenter.DelHandler(GTEventID.TYPE_UP_BOSS_HP, ShowBossHP);
    }

    private void ShowBossHP()
    {
        Character boss = CharacterManager.Boss;
        if (boss == null)
        {
            m_HPSlider.value = 1;
        }
        else
        {
            m_HPSlider.value = (boss.CurrAttr.HP * 1f) / boss.CurrAttr.MaxHP;
        }
    }
}
