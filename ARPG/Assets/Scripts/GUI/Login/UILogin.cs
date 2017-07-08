
using UnityEngine;
using System.Collections;
using System;
using Protocol;

public class UILogin : GTWindow
{
    private GameObject       state1;
    private GameObject       state2;
    private GameObject       state3;

    private GameObject       btnNotice;
    private GameObject       btnLoginGame;
    private GameObject       btnServer;

    private GameObject       btnAccount;
    private GameObject       btnAccountLogin;


    private UIToggle         btnStandalone;
    private UIToggle         btnNetwork;
    private GameObject       btnNext;

    private UILabel          versionNumber;
    private UILabel          curServerName;
    private UILabel          curAccount;
    private LoginViewMode    loginMode;

    public enum LoginViewMode
    {
        State1,
        State2,
        State3,
    }

    public UILogin()
    {
        mResident = false;
        mResPath = "Login/UILogin";
        Type = EWindowType.WINDOW;
    }

    protected override void OnAwake()
    {
        state1 = transform.Find("State1").gameObject;
        state2 = transform.Find("State2").gameObject;
        state3 = transform.Find("State3").gameObject;

        btnStandalone = state1.transform.Find("Btn_Standalone").GetComponent<UIToggle>();
        btnNetwork = state1.transform.Find("Btn_Network").GetComponent<UIToggle>();
        btnNext = state1.transform.Find("Btn_Next").gameObject;

        btnAccount = state2.transform.Find("Btn_Account").gameObject;
        btnAccountLogin = state2.transform.Find("Btn_AccountLogin").gameObject;

        btnNotice = state3.transform.Find("Btn_Notice").gameObject;
        btnLoginGame = state3.transform.Find("Btn_LoginGame").gameObject;
        btnServer  = state3.transform.Find("Btn_Server").gameObject;

        curServerName = btnServer.transform.Find("Label").GetComponent<UILabel>();
        curAccount  = btnAccount.transform.Find("Label").GetComponent<UILabel>();
        versionNumber = transform.Find("Bottom/Version").GetComponent<UILabel>();

        int group = GTWindowManager.Instance.GetToggleGroupId();
        btnStandalone.group = group;
        btnNetwork.group = group;
        btnStandalone.value = true;
    }

    protected override void OnAddButtonListener()
    {
        UIEventListener.Get(btnAccount).onClick = OnAccountClick;
        UIEventListener.Get(btnAccountLogin).onClick = OnAccountLoginClick;

        UIEventListener.Get(btnNotice).onClick = OnNoticeClick;
        UIEventListener.Get(btnLoginGame).onClick = OnLoginGameClick;
        UIEventListener.Get(btnServer).onClick = OnServerClick;


        UIEventListener.Get(btnStandalone.gameObject).onClick = OnMenuStandaloneClick;
        UIEventListener.Get(btnNetwork.gameObject).onClick = OnMenuNetworkClick;
        UIEventListener.Get(btnNext).onClick = OnNextClick;
    }



    protected override void OnAddHandler()
    {
        GTEventCenter.AddHandler(GTEventID.TYPE_LOGINGAME_CALLBACK,  OnRecvLoginGame);
        GTEventCenter.AddHandler(GTEventID.TYPE_REGISTER_CALLBACK,   OnRecvRegister);
        GTEventCenter.AddHandler(GTEventID.TYPE_ACCLOGIN_CALLBACK,   OnRecvAccLogin);
        GTEventCenter.AddHandler(GTEventID.TYPE_GETSERVERS_CALLBACK, OnRecvGetServers);
        GTEventCenter.AddHandler(GTEventID.TYPE_SELECTSERVER,        OnRecvSelectServer);
    }

    protected override void OnDelHandler()
    {
        GTEventCenter.DelHandler(GTEventID.TYPE_LOGINGAME_CALLBACK,  OnRecvLoginGame);
        GTEventCenter.DelHandler(GTEventID.TYPE_REGISTER_CALLBACK,   OnRecvRegister);
        GTEventCenter.DelHandler(GTEventID.TYPE_ACCLOGIN_CALLBACK,   OnRecvAccLogin);
        GTEventCenter.DelHandler(GTEventID.TYPE_GETSERVERS_CALLBACK, OnRecvGetServers);
        GTEventCenter.DelHandler(GTEventID.TYPE_SELECTSERVER,        OnRecvSelectServer);
    }

    protected override void OnEnable()
    {
        this.loginMode = LoginViewMode.State1;
        GTGlobal.Along = true;
        this.InitModel();
        this.InitView();
        this.InitGameModeView();
        this.ShowCurrServer();
        this.ShowUsernameAndPassword();
    }

    protected override void OnClose()
    {
        GTCameraManager.Instance.RevertMainCamera();
    }

    private void InitModel()
    {
        KTransform param = KTransform.Create(Vector3.zero,Vector3.zero);
        Camera camera = GTCameraManager.Instance.MainCamera;
        camera.transform.position    = new Vector3(0.2214f, 1.5794f, 41.2714f);
        camera.transform.eulerAngles = new Vector3(-23.7111f, -179.525f, 0.01245f);
        camera.fieldOfView = 46.6f;
        camera.renderingPath = RenderingPath.DeferredLighting;
    }

    private void InitView()
    {
        versionNumber.text = GTTools.Format("版本号：{0}", Application.version);
    }

    private void InitGameModeView()
    {
        state1.SetActive(loginMode == LoginViewMode.State1);
        state2.SetActive(loginMode == LoginViewMode.State2);
        state3.SetActive(loginMode == LoginViewMode.State3);

        btnNotice. SetActive(!GTGlobal.Along);
        btnServer. SetActive(!GTGlobal.Along);
    }

    private void OnNextClick(GameObject go)
    {
        if(GTGlobal.Along)
        {
            NetworkManager.Instance.AddMessageCenterHandler();
            this.loginMode = LoginViewMode.State3;
            GTXmlHelper.Write = true;
        }
        else
        {
            this.loginMode = LoginViewMode.State2;
            GTXmlHelper.Write = false;
            NetworkManager.Instance.ConnectLoginServer();
        }
        this.InitGameModeView();
    }

    private void OnMenuNetworkClick(GameObject go)
    {
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
        GTGlobal.Along = false;
    }

    private void OnMenuStandaloneClick(GameObject go)
    {
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
        GTGlobal.Along = true;
    }

    private void OnServerClick(GameObject go)
    {
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
        LoginService.Instance.TryGetSvrList();
    }

    private void OnLoginGameClick(GameObject go)
    {
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
        XServer node = LoginModule.Instance.GetCurrServer();
        LoginService.Instance.TrySelectServer(node == null ? 0 : node.ID);
    }

    private void OnAccountClick(GameObject go)
    {
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
        GTWindowManager.Instance.OpenWindow(EWindowID.UIAccount);
    }

    private void OnAccountLoginClick(GameObject go)
    {
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
        LoginService.Instance.TryLogin(LoginModule.Instance.LastUsername, LoginModule.Instance.LastPassword);
    }

    private void OnNoticeClick(GameObject go)
    {
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
        GTWindowManager.Instance.OpenWindow(EWindowID.UINotice);
    }

    private void OnRecvLoginGame()
    {
        this.Hide();
    }

    private void OnRecvRegister()
    {
        this.ShowUsernameAndPassword();
    }

    private void OnRecvAccLogin()
    {
        this.loginMode = LoginViewMode.State3;
        this.InitGameModeView();
        this.ShowCurrServer();
    }

    private void OnRecvGetServers()
    {
        GTWindowManager.Instance.OpenWindow(EWindowID.UIServer);
    }

    private void OnRecvSelectServer()
    {
        ShowCurrServer();
    }

    public void ShowCurrServer()
    {
        XServer data = LoginModule.Instance.GetCurrServer();
        curServerName.text = data == null ? "服务器" : data.Name;
    }

    public void ShowUsernameAndPassword()
    {
        string username = LoginModule.Instance.LastUsername;
        curAccount.text = string.IsNullOrEmpty(username) ? "帐号" : username;
    }
}
