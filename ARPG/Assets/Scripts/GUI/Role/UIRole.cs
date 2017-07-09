using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Protocol;

public class UIRole : GTWindow
{
    private UILabel         labHeroName;
    private UIInput         iptHeroName;
    private GameObject      btnEnterGame;
    private GameObject      btnCreateRole;
    private GameObject      btnRoll;
    private GameObject      btnHero;

    private GameObject      roleTemplate;
    private UIGrid          roleGrid;

    private int             mRoleIndex      = 0;
    private Character       mPlayer         = null;
    private Vector3         mRoleModelPos   = new Vector3(-44.17f, 2.473f, -4.41f);
    private Vector3         mRoleModelEuler = new Vector3(0, 180, 0);
    private List<DRole>     mRoleDBList;

    private DRole           CurRole
    {
        get { return mRoleDBList[mRoleIndex]; }
    }

    public UIRole()
    {
        Type        = EWindowType.WINDOW;
        mResident   = false;
        mResPath    = "Role/UIRole";
    }

    protected override void OnAwake()
    {
        labHeroName = transform.Find("HeroName/Label").GetComponent<UILabel>();
        Transform left = transform.Find("Left");
        Transform right = transform.Find("Right");

        iptHeroName = transform.Find("Bottom/NameInput").GetComponent<UIInput>();
        btnCreateRole = transform.Find("Bottom/Btn_CreateRole").gameObject;
        btnRoll = transform.Find("Bottom/Btn_Roll").gameObject;
        btnEnterGame = transform.Find("Bottom/Btn_EnterGame").gameObject;
        btnHero = transform.Find("HeroTexture").gameObject;
        roleTemplate = transform.Find("Template").gameObject;
        roleTemplate.SetActive(false);
        roleGrid = transform.Find("View/Grid").GetComponent<UIGrid>();
    }

    protected override void OnAddButtonListener()
    {
        UIEventListener.Get(btnCreateRole).onClick = OnCreateRoleClick;
        UIEventListener.Get(btnEnterGame).onClick =OnEnterGameClick;
        UIEventListener.Get(btnRoll).onClick = OnRollClick;
        UIEventListener.Get(btnHero.gameObject).onDrag = OnHeroTextureDrag;
    }

    protected override void OnAddHandler()
    {
        GTEventCenter.AddHandler(GTEventID.TYPE_CREATEROLE_CALLBACK, OnRecvCreateRole);
        GTEventCenter.AddHandler(GTEventID.TYPE_ENTERGAME_CALLBACK, OnRecvEnterGame);
    }

    protected override void OnDelHandler()
    {
        GTEventCenter.DelHandler(GTEventID.TYPE_CREATEROLE_CALLBACK, OnRecvCreateRole);
        GTEventCenter.DelHandler(GTEventID.TYPE_ENTERGAME_CALLBACK, OnRecvEnterGame);
    }


    private void OnHeroTextureDrag(GameObject go, Vector2 delta)
    {
        if (mPlayer == null)
        {
            return;
        }
        ESpin.Get(mPlayer.Obj).OnSpin(delta, 2);
    }

    private void OnCreateRoleClick(GameObject go)
    {
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
        string name = iptHeroName.value.Trim();
        LoginService.Instance.TryCreateRole(name, CurRole.Id, LoginModule.Instance.LastAccountID);
    }

    private void OnEnterGameClick(GameObject go)
    {
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
        int id = mRoleDBList[mRoleIndex].Id;
        XCharacter c = DataDBSRole.GetDataById(id);
        if(c == null)
        {
            GTItemHelper.ShowTip("你还没有创建这个角色");
            return;
        }
        LoginService.Instance.TryEnterGame(c.GUID);
    }

    private void OnRollClick(GameObject go)
    {
        GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
        string randomName = RoleModule.Instance.GetRandomName();
        iptHeroName.value = randomName;
    }

    private void OnRecvEnterGame()
    {
        Hide();
    }

    private void OnRecvCreateRole()
    {
        ShowView();
    }

    protected override void OnEnable()
    {
        mRoleDBList = new List<DRole>(ReadCfgRole.Dict.Values);
        InitModel();
        InitView();
        ShowView();
        ShowModel();
    }

    protected override void OnClose()
    {
        mRoleIndex = 0;
        CharacterManager.Instance.DelActor(mPlayer);
        mPlayer = null;
        GTCameraManager.Instance.RevertMainCamera();
    }

    private void ShowView()
    {
        int id = mRoleDBList[mRoleIndex].Id;
        XCharacter role = DataDBSRole.GetDataById(id);
        DActor roleDB = ReadCfgActor.GetDataById(id);
        btnCreateRole.SetActive(role == null);
        btnRoll.SetActive(role == null);
        iptHeroName.gameObject.SetActive(role == null);
        btnEnterGame.SetActive(role != null);
        labHeroName.text = role != null ? GTTools.Format("Lv.{0} {1}", role.Level, role.Name != null ? role.Name : roleDB.Name) : "未创建";
    }

    private void InitModel()
    {
        Camera camara = GTCameraManager.Instance.MainCamera;
        camara.transform.position = new Vector3(-44.05f, 4.55f, -15.2f);
        camara.transform.eulerAngles = new Vector3(6.8639f, -0.4323f, 0.0958f);
        camara.renderingPath = RenderingPath.UsePlayerSettings;
        camara.fieldOfView = 36f;
        camara.clearFlags = CameraClearFlags.Skybox;
    }

    private void InitView()
    {
        int group = GTWindowManager.Instance.GetToggleGroupId();
        for (int i = 0; i < mRoleDBList.Count; i++)
        {
            int id = mRoleDBList[i].Id;
            int index = i;
            DActor roleDB = ReadCfgActor.GetDataById(id);
            GameObject item = NGUITools.AddChild(roleGrid.gameObject, roleTemplate);
            item.SetActive(true);
            roleGrid.AddChild(item.transform);
            UISprite roleIcon = item.transform.Find("Icon").GetComponent<UISprite>();
            roleIcon.spriteName = roleDB.Icon;

            UIToggle toggle = item.GetComponent<UIToggle>();
            toggle.group = group;
            toggle.value = 0 == mRoleIndex;
            UIEventListener.Get(item).onClick = delegate
            {
                GTAudioManager.Instance.PlayEffectAudio(GTAudioKey.SOUND_UI_CLICK);
                if (mRoleIndex == index)
                {
                    return;
                }
                mRoleIndex = index;
                ShowView();
                ShowModel();
                ShowMask();
                mPlayer.Command.Get<CommandUseSkill>().Update(ESkillPos.Skill_9).Do();
            };
        }
    }

    private void ShowMask()
    {

    }

    private void ShowModel()
    {
        if (mPlayer != null)
        {
            CharacterManager.Instance.DelActor(mPlayer);
        }
        KTransform param = KTransform.Default;

        DRole roleDB = mRoleDBList[mRoleIndex];
        int id = roleDB.Id;
        mPlayer = CharacterManager.Instance.AddRole(id, param);
        mPlayer.EnableCharacter(false);
        mPlayer.EnableRootMotion(false);
        mPlayer.Action.Play("idle");
        if (roleDB.DisplayWeapon > 0)
        {
            mPlayer.Avatar.ChangeAvatar(8, roleDB.DisplayWeapon);
        }
        mPlayer.CacheTransform.localPosition = mRoleModelPos;
        mPlayer.CacheTransform.localEulerAngles = mRoleModelEuler;
        GameObject go = mPlayer.Obj;
        go.transform.position = mRoleModelPos;
        go.transform.eulerAngles = mRoleModelEuler;
        go.SetActive(true);
    }

    private string ShowStar(int star)
    {
        string s = string.Empty;
        for (int i = 0; i < star; i++)
        {
            s = GTTools.Format("{0}{1}", s, "★");
        }
        return s;
    }
}
