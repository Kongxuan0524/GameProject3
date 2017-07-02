
using UnityEngine;
using System.Collections;
using System;
using Protocol;

public class UILogin : GTWindow
{
    private GameObject       btnNotice;
    private GameObject       btnLoginGame;
    private GameObject       btnAccount;
    private GameObject       btnServer;
    private UILabel          curServerName;
    private UILabel          curAccount;

    private UIToggle menuStandalone;
    private UIToggle menuNetwork;

    private UILabel          mVersion;
    private CharacterAvatar  mLoginMount;


    public UILogin()
    {
        mResident = false;
        mResPath = "Login/UILogin";
        Type = EWindowType.WINDOW;
    }

    protected override void OnAwake()
    {
        btnAccount = transform.Find("Buttons/Btn_Account").gameObject;
        btnNotice = transform.Find("Buttons/Btn_Notice").gameObject;
        btnLoginGame = transform.Find("Buttons/Btn_LoginGame").gameObject;
        btnServer  = transform.Find("Buttons/Btn_Server").gameObject;
        curServerName = btnServer.transform.Find("Label").GetComponent<UILabel>();
        curAccount  = btnAccount.transform.Find("Label").GetComponent<UILabel>();

        mVersion = transform.Find("Bottom/Version").GetComponent<UILabel>();
        menuStandalone = transform.Find("Menus/Menu_Standalone").GetComponent<UIToggle>();
        menuNetwork    = transform.Find("Menus/Menu_Network").GetComponent<UIToggle>();

        int group = GTWindowManager.Instance.GetToggleGroupId();
        menuStandalone.group = group;
        menuNetwork.group = group;
        menuStandalone.value = true;
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
        mVersion.text = GTTools.Format("版本号：{0}", Application.version);
    }

    private void InitGameModeView()
    {
        btnAccount.SetActive(!NetworkManager.Instance.Alone);
        btnNotice.SetActive(!NetworkManager.Instance.Alone);
        btnServer.SetActive(!NetworkManager.Instance.Alone);
    }

    protected override void OnAddButtonListener()
    {
        UIEventListener.Get(btnAccount).onClick = OnAccountClick;
        UIEventListener.Get(btnNotice).onClick = OnNoticeClick;
        UIEventListener.Get(btnLoginGame).onClick = OnLoginGameClick;
        UIEventListener.Get(btnServer).onClick = OnServerClick;
        UIEventListener.Get(menuStandalone.gameObject).onClick = OnMenuStandaloneClick;
        UIEventListener.Get(menuNetwork.gameObject).onClick    = OnMenuNetworkClick;
    }

    private void OnMenuNetworkClick(GameObject go)
    {
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
        NetworkManager.Instance.Alone = false;
        InitGameModeView();
    }

    private void OnMenuStandaloneClick(GameObject go)
    {
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
        NetworkManager.Instance.Alone = true;
        InitGameModeView();
    }

    private void OnServerClick(GameObject go)
    {
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
        GTWindowManager.Instance.OpenWindow(EWindowID.UI_SERVER);
    }

    private void OnLoginGameClick(GameObject go)
    {
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
        LoginService.Instance.TryLoginGame();
    }

    private void OnAccountClick(GameObject go)
    {
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
        GTWindowManager.Instance.OpenWindow(EWindowID.UI_ACCOUNT);
    }

    private void OnNoticeClick(GameObject go)
    {
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
        GTWindowManager.Instance.OpenWindow(EWindowID.UI_NOTICE);
    }

    protected override void OnAddHandler()
    {
        GTEventCenter.AddHandler(GTEventID.TYPE_LOGINGAME_CALLBACK, OnRecvLoginGame);
    }

    protected override void OnDelHandler()
    {
        GTEventCenter.DelHandler(GTEventID.TYPE_LOGINGAME_CALLBACK, OnRecvLoginGame);
    }

    protected override void OnEnable()
    {
        InitModel();
        InitView();
        InitGameModeView();
        ShowCurrServer();
        ShowUsernameAndPassword();
    }

    protected override void OnClose()
    {
        GTCameraManager.Instance.RevertMainCamera();
    }

    private void OnRecvLoginGame()
    {
        Hide();
    }

    public void ShowCurrServer()
    {
        XServer data = LoginModule.Instance.GetCurrServer();
        if (data == null)
        {
            curServerName.text = "服务器";
        }
        else
        {
            curServerName.text = data.Name;
        }
    }

    public void ShowUsernameAndPassword()
    {
        if(string.IsNullOrEmpty(LoginModule.Instance.LastUsername))
        {
            curAccount.text = "帐号";
        }
        else
        {
            curAccount.text = LoginModule.Instance.LastUsername;
        }
    }
}
