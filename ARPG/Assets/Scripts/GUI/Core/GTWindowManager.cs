using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using UnityEngine;


public class GTWindowManager : GTSingleton<GTWindowManager>
{
    private Dictionary<EWindowID, GTWindow>         mAllWindows = new Dictionary<EWindowID, GTWindow>();
    private Dictionary<EWindowType, List<GTWindow>> mOpenWindows = new Dictionary<EWindowType, List<GTWindow>>();
    private Dictionary<EWindowType, int>            mMinDepths = new Dictionary<EWindowType, int>();
    private List<GTWindow>                          mOpenWinStack = new List<GTWindow>();
    private int mToggleGroupId = 1;

    public GTWindowManager()
    {
        this.SetAllDepth();
        this.RegisterWindows();
    }

    void SetAllDepth()
    {
        mMinDepths[EWindowType.BOTTOM] = 100;
        mMinDepths[EWindowType.FLYWORD] = 200;
        mMinDepths[EWindowType.FIXED]  = 500;
        mMinDepths[EWindowType.WINDOW] = 700;
        mMinDepths[EWindowType.DIALOG] = 900;
        mMinDepths[EWindowType.MSGTIP] = 2000;
        mMinDepths[EWindowType.LOADED] = 4000;
        mMinDepths[EWindowType.MASKED] = 5000;
    }

    void RegisterWindows()
    {
        RegisterWindow(EWindowID.UILogin, new UILogin());
        RegisterWindow(EWindowID.UIServer, new UIServer());
        RegisterWindow(EWindowID.UINotice, new UINotice());
        RegisterWindow(EWindowID.UIAccount, new UIAccount());
        RegisterWindow(EWindowID.UINetWaiting, new UINetWaiting());

        RegisterWindow(EWindowID.UICreateRole, new UIRole());
        RegisterWindow(EWindowID.UIHome, new UIHome());
        RegisterWindow(EWindowID.UIHeroInfo, new UIHeroInfo());
        RegisterWindow(EWindowID.UISetting, new UISetting());

        RegisterWindow(EWindowID.UIBag, new UIBag());
        RegisterWindow(EWindowID.UIRoleEquip, new UIRoleEquip());
        RegisterWindow(EWindowID.UIRoleGem, new UIRoleGem());
        RegisterWindow(EWindowID.UIRoleFashion, new UIRoleFashion());
        RegisterWindow(EWindowID.UIRoleRune, new UIRoleRune());
        RegisterWindow(EWindowID.UIRoleFetter, new UIRoleFetter());

        RegisterWindow(EWindowID.UIItemInfo, new UIItemInfo());
        RegisterWindow(EWindowID.UIItemUse, new UIItemUse());
        RegisterWindow(EWindowID.UIEquipInfo, new UIEquipInfo());
        RegisterWindow(EWindowID.UIGemInfo, new UIGemInfo());
        RegisterWindow(EWindowID.UIChipInfo, new UIChipInfo());
        RegisterWindow(EWindowID.UIEquip, new UIEquip());
        RegisterWindow(EWindowID.UIGem, new UIGem());

        RegisterWindow(EWindowID.UIMessageTip, new UIMessageTip());
        RegisterWindow(EWindowID.UIMessageBox, new UIMessageBox());
        RegisterWindow(EWindowID.UIMessageBoxForNetwork, new UIMessageBoxForNetwork());
        RegisterWindow(EWindowID.UIAwardTip, new UIAwardTip());
        RegisterWindow(EWindowID.UIDialogue, new UIDialogue());

        RegisterWindow(EWindowID.UIMainRaid, new UIMainRaid());
        RegisterWindow(EWindowID.UIMainGate, new UIMainGate());
        RegisterWindow(EWindowID.UIMainResult, new UIMainResult());

        RegisterWindow(EWindowID.UIAwardBox, new UIAwardBox());
        RegisterWindow(EWindowID.UILoading,  new UILoading());

        RegisterWindow(EWindowID.UIMount, new UIMount());
        RegisterWindow(EWindowID.UIMountLibrary, new UIMountLibrary());
        RegisterWindow(EWindowID.UIMountBlood, new UIMountBlood());
        RegisterWindow(EWindowID.UIMountTame, new UIMountTame());
        RegisterWindow(EWindowID.UIMountTurned, new UIMountTurned());

        RegisterWindow(EWindowID.UIRelics, new UIRelics());
        RegisterWindow(EWindowID.UIStore, new UIStore());
        RegisterWindow(EWindowID.UIPet, new UIPet());

        RegisterWindow(EWindowID.UIPartner, new UIPartner());
        RegisterWindow(EWindowID.UIPartnerBattle, new UIPartnerBattle());
        RegisterWindow(EWindowID.UIPartnerStrength, new UIPartnerStrength());
        RegisterWindow(EWindowID.UIPartnerAdvance, new UIPartnerAdvance());
        RegisterWindow(EWindowID.UIPartnerFetter, new UIPartnerFetter());
        RegisterWindow(EWindowID.UIPartnerProperty, new UIPartnerProperty());
        RegisterWindow(EWindowID.UIPartnerSkill, new UIPartnerSkill());
        RegisterWindow(EWindowID.UIPartnerStar, new UIPartnerStar());
        RegisterWindow(EWindowID.UIPartnerWake, new UIPartnerWake());
        RegisterWindow(EWindowID.UIPartnerWash, new UIPartnerWash());

        RegisterWindow(EWindowID.UITask, new UITask());
        RegisterWindow(EWindowID.UITaskTalk, new UITaskTalk());
        RegisterWindow(EWindowID.UIWorldMap, new UIWorldMap());

        RegisterWindow(EWindowID.UIPlotCutscene, new UIPlot());
        RegisterWindow(EWindowID.UIGuide, new UIGuide());

        RegisterWindow(EWindowID.UIAdventure, new UIAdventure());
        RegisterWindow(EWindowID.UISkill, new UISkill());
        RegisterWindow(EWindowID.UIReborn, new UIReborn());
        RegisterWindow(EWindowID.UIMainBossHP, new UIMainBossHP());
    }

    void RegisterWindow(EWindowID id, GTWindow win)
    { 
        mAllWindows[id] = win;
        win.ID = id;
    }

    void FindPanels(GTWindow window,ref List<UIPanel> panels)
    {
        if (window == null||window.Root==null)
        {
            return;
        }
        panels.AddRange(window.Root.GetComponents<UIPanel>());
        panels.AddRange(window.Root.GetComponentsInChildren<UIPanel>());
    }

    void RefreshDepth(GTWindow window)
    {
        EWindowType type = window.Type;
        List<UIPanel> pList = new List<UIPanel>();
        FindPanels(window, ref pList);

        List<UIPanel> aList = new List<UIPanel>();
        List<GTWindow> windows = mOpenWindows[type];
        for (int i = 0; i < windows.Count; i++)
        {
            FindPanels(windows[i], ref aList);
        }
        for (int i = aList.Count - 1; i >= 0; i--)
        {
            if (aList[i] == null || aList[i].transform == null)
            {
                aList.RemoveAt(i);
            }
        }
        if (pList.Count >= 2)
        {
            pList.Sort(UIPanel.CompareFunc);
        }
        int stDepth = mMinDepths[type];
        aList.ForEach(item => { stDepth = item.depth > stDepth ? item.depth : stDepth; });
        pList.ForEach(item => { stDepth += 2; item.depth = stDepth; });
        UIPanel.list.Sort(UIPanel.CompareFunc);
    }

    void DealWindowStack(GTWindow win,bool open)
    {
        if (win.Type != EWindowType.WINDOW)
        {
            return;
        }
        if (open)
        {
            for (int i = 0; i < mOpenWinStack.Count; i++)
            {
                if (mOpenWinStack[i] != win)
                {
                    mOpenWinStack[i].CacheTransform.gameObject.SetActive(false);
                }
            }
            mOpenWinStack.Add(win);
        }
        else
        {
            mOpenWinStack.Remove(win);
            if (mOpenWinStack.Count > 0)
            {
                GTWindow last = mOpenWinStack[mOpenWinStack.Count - 1];
                last.CacheTransform.gameObject.SetActive(true);
            }
        }
    }

    void DealMask()
    {

    }

    void CloseAllWindowByType(EWindowType type)
    {
        List<GTWindow> list = null;
        mOpenWindows.TryGetValue(type, out list);
        if (list == null) return;
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].HasParentWindow)
            {
                list[i].Hide();
            }
        }
    }

    public GTWindow OpenWindow(EWindowID windowID)
    {
        if (!mAllWindows.ContainsKey(windowID))
        {
            return null;
        }
        GTWindow window = mAllWindows[windowID];
        DealWindowStack(window, true);
        window.Show();
        Transform trans = window.CacheTransform;
        if (trans == null)
            return null;
        if (window.Type == EWindowType.WINDOW)
        {
            CloseAllWindowByType(EWindowType.DIALOG);
        }
        if (window.Type == EWindowType.WINDOW || window.Type == EWindowType.DIALOG)
        {
            GTEventCenter.FireEvent(GTEventID.TYPE_STOP_JOYSTICK);
        }
        GTCameraManager.Instance.AddUI(trans.gameObject);
        List<GTWindow> list = null;
        mOpenWindows.TryGetValue(window.Type, out list);
        if (list == null)
        {
            list = new List<GTWindow>();
            mOpenWindows[window.Type] = list;
        }
        list.Add(window);
        RefreshDepth(window);
        return window;
    }

    public void     CloseWindow(EWindowID windowID)
    {
        GTWindow window = mAllWindows[windowID];
        if (window == null) return;
        EWindowType type = window.Type;
        window.Close();
        List<GTWindow> list = null;
        mOpenWindows.TryGetValue(type, out list);
        if (list != null)
        {
            list.Remove(window);
        }
        DealWindowStack(window, false);
    }

    public GTWindow GetWindow(EWindowID windowID)
    {
        GTWindow window = null;
        mAllWindows.TryGetValue(windowID, out window);
        return window;
    }

    public T GetWindow<T>(EWindowID windowID) where T : GTWindow
    {
        GTWindow baseWindow = null;
        mAllWindows.TryGetValue(windowID, out baseWindow);
        T window = (T)baseWindow;
        return window;
    }

    public int GetToggleGroupId()
    {
        mToggleGroupId++;
        return mToggleGroupId;
    }

    public void LockNGUI(bool lockNGUI)
    {
        if(lockNGUI)
        {
            GTCameraManager.Instance.NGUICamera.GetComponent<UICamera>().eventReceiverMask = (1 << GTLayer.LAYER_DEFAULT);
        }
        else
        {
            GTCameraManager.Instance.NGUICamera.GetComponent<UICamera>().eventReceiverMask = (1 << GTLayer.LAYER_UI);
        }
    }

    public void Release()
    {
        mToggleGroupId = 1;
        foreach (KeyValuePair<EWindowID, GTWindow> pair in mAllWindows)
        {
            if (pair.Key != EWindowID.UILoading)
            {
                pair.Value.Close();
            }
        }
        mOpenWindows.Clear();
        mOpenWinStack.Clear();
    }
}