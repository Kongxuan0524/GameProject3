using UnityEngine;
using System.Collections;
using System;

public class UIAccount : GTWindow
{
    private GameObject  btnClose;
    private GameObject  btnSure;
    private GameObject  btnRegisterAndLogin;
    private GameObject  btnCancel;
    private GameObject  btnRegister;
    private UIInput     usernameInput;
    private UIInput     passwordInput;

    private bool        showRegister;
    


    public UIAccount()
    {
        mResident = false;
        mResPath = "Login/UIAccount";
        Type = EWindowType.DIALOG;
    }

    protected override void OnAwake()
    {
        btnClose = transform.Find("Pivot/BtnClose").gameObject;
        btnSure = transform.Find("Pivot/BtnSure").gameObject;
        btnRegisterAndLogin = transform.Find("Pivot/BtnRegisterAndLogin").gameObject;
        btnRegister = transform.Find("Pivot/BtnRegister").gameObject;
        btnCancel = transform.Find("Pivot/BtnCancel").gameObject;
        usernameInput = transform.Find("Pivot/Username/Input").GetComponent<UIInput>();
        passwordInput = transform.Find("Pivot/Password/Input").GetComponent<UIInput>();
    }

    protected override void OnAddButtonListener()
    {
        UIEventListener.Get(btnClose).onClick = OnCloseClick;
        UIEventListener.Get(btnSure).onClick = OnSureClick;
        UIEventListener.Get(btnRegisterAndLogin).onClick = OnRegisterAndLoginClick;
        UIEventListener.Get(btnRegister).onClick = OnRegisterClick;
        UIEventListener.Get(btnCancel).onClick = OnBtnCancelClick;
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
        this.showRegister = false;
        this.InitView();
    }

    private void OnCloseClick(GameObject go)
    {
        this.Hide();
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_BACK);
    }

    private void OnBtnCancelClick(GameObject go)
    {
        this.showRegister = false;
        this.InitView();
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
    }

    private void OnRegisterClick(GameObject go)
    {
        this.showRegister = true;
        this.InitView();
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
    }

    private void OnRegisterAndLoginClick(GameObject go)
    {
        string username = this.usernameInput.value;
        string password = this.passwordInput.value;
        LoginService.Instance.TryRegister(username, password);
        Hide();
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_BACK);
    }

    private void OnSureClick(GameObject go)
    {
        string username = this.usernameInput.value;
        string password = this.passwordInput.value;
        LoginService.Instance.TryLogin(username, password);
        Hide();
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_BACK);
    }

    private void InitView()
    {
        if(showRegister)
        {
            btnSure.SetActive(true);
            btnRegister.SetActive(true);
            btnRegisterAndLogin.SetActive(false);
            btnCancel.SetActive(false);
        }
        else
        {
            btnSure.SetActive(false);
            btnRegister.SetActive(false);
            btnRegisterAndLogin.SetActive(true);
            btnCancel.SetActive(true);
        }
    }
}
