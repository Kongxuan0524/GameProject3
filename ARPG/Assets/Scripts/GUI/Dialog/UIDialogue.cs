using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIDialogue : GTWindow
{
    private GameObject btnNext;
    private GameObject btnSkip;
    private UILabel    dlgTitle;
    private UILabel    dlgContent;
    private UITexture  dlgTexture;
    private int        dlgStID;
    private int        dlgEdID;
    private int        dlgCurID;
    private Callback   onFinish;
 

    public UIDialogue()
    {
        mResident = true;
        mResPath = "Common/UIDialogue";
        Type = EWindowType.DIALOG;
    }

    protected override void OnAwake()
    {
        Transform bottom = transform.Find("Bottom");
        Transform top    = transform.Find("Top");
        this.btnNext     = bottom.Find("BtnNext").gameObject;
        this.btnSkip     = top.Find("BtnSkip").gameObject;
        this.dlgTitle    = bottom.Find("DlgTitle").GetComponent<UILabel>();
        this.dlgContent  = bottom.Find("DlgContent").GetComponent<UILabel>();
        this.dlgTexture  = bottom.Find("DlgTexture").GetComponent<UITexture>();
    }

    protected override void OnEnable()
    {

    }

    protected override void OnClose()
    {

    }

    protected override void OnAddButtonListener()
    {
        UIEventListener.Get(btnNext).onClick = OnBtnNextClick;
        UIEventListener.Get(btnSkip).onClick = OnBtnSkipClick;
    }

    protected override void OnAddHandler()
    {
        
    }

    protected override void OnDelHandler()
    {
        
    }

    private void OnBtnNextClick(GameObject go)
    {
        this.GoNext();
        this.ShowDialogueContent();
    }

    private void OnBtnSkipClick(GameObject go)
    {
        if (onFinish != null)
        {
            onFinish();
            onFinish = null;
        }
        Hide();
    }

    private void GoNext()
    {
        if (this.dlgCurID == this.dlgEdID)
        {
            if (onFinish != null)
            {
                onFinish();
                onFinish = null;
            }
            Hide();
            return;
        }

        if (this.dlgCurID < dlgStID)
        {
            this.dlgCurID = dlgStID;
        }
        else
        {
            this.dlgCurID++;
        }

    }

    private void ShowDialogueContent()
    {
        DDialogue db = ReadCfgDialogue.GetDataById(dlgCurID);
        if (db == null)
        {
            return;
        }
        if (db.Actor == 0)
        {
            Character c = CharacterManager.Main;
            dlgTitle.text = c == null ? string.Empty : c.Name;
        }
        else
        {
            DActor actorDB = ReadCfgActor.GetDataById(db.Actor);
            dlgTitle.text = actorDB == null ? string.Empty : actorDB.Name;
        }

        string replaceName = string.Empty;
        switch(db.ContentType)
        {
            case EDialogueContentType.TYPE_NONE:
                {
                    replaceName = string.Empty;
                }
                break;
            case EDialogueContentType.TYPE_PLAYER:
                {
                    Character c = CharacterManager.Main;
                    replaceName = c == null ? string.Empty : c.Name;
                }
                break;
            case EDialogueContentType.TYPE_ACTOR:
                {
                    DActor actorDB = ReadCfgActor.GetDataById(db.ContentID);
                    replaceName = actorDB == null ? string.Empty : actorDB.Name;
                }
                break;
            case EDialogueContentType.TYPE_ITEM:
                {
                    DItem itemDB = ReadCfgItem.GetDataById(db.ContentID);
                    replaceName = itemDB == null ? string.Empty : itemDB.Name;
                }
                break;
            case EDialogueContentType.TYPE_MAP:
                {
                    DWorld worldDB = ReadCfgWorld.GetDataById(db.ContentID);
                    replaceName = worldDB == null ? string.Empty : worldDB.Name;
                }
                break;
        }
        dlgContent.text = db.Content.Replace("%p%", replaceName);

        switch (db.ContentShowType)
        {
            case EDialogueContentShowType.Normal:
                dlgContent.GetComponent<TypewriterEffect>().enabled = false;
                break;
            case EDialogueContentShowType.Effect:
                dlgContent.GetComponent<TypewriterEffect>().enabled = true;
                dlgContent.GetComponent<TypewriterEffect>().ResetToBeginning();
                break;
        }

    }

    public void  ShowDialogue(int st, int ed, bool isCanSkip, Callback onFinish)
    {
        this.dlgStID = st;
        this.dlgEdID = ed;
        this.onFinish = onFinish;
        this.GoNext();
        this.ShowDialogueContent();
        this.btnSkip.SetActive(isCanSkip);
    }

}