using UnityEngine;
using System.Collections;
using Protocol;
using System;

public class LoginService : GTSingleton<LoginService>
{
    public void TryRegister(string username, string password)
    {
        AccountRegReq req = new AccountRegReq();
        req.AccountName = username;
        req.Password = password;
        LoginModule.Instance.LastUsername = username;
        LoginModule.Instance.LastPassword = password;
        NetworkManager.Instance.Send(MessageID.MSG_ACCOUNT_REG_REQ, req);
    }

    public void TryLogin(string username, string password)
    {
        AccountLoginReq req = new AccountLoginReq();
        req.AccountName = username;
        req.Password = password;
        NetworkManager.Instance.Send(MessageID.MSG_ACCOUNT_LOGIN_REQ, req, 0, 0);
    }

    public void TryGetSvrList()
    {
        ClientServerListReq req = new ClientServerListReq();
        req.AccountID = 1;
        req.Channel = 2;
        req.ClientVersion = 100001;
        NetworkManager.Instance.Send(MessageID.MSG_SERVER_LIST_REQ, req, 0, 0);
    }

    //确认当前服务器
    public void TrySelectServer(Int32 ServerID)
    {
        SelectServerReq req = new SelectServerReq();
        req.ServerID = ServerID;
        NetworkManager.Instance.Send(MessageID.MSG_SELECT_SERVER_REQ, req);
    }

    //创建角色
    public void TryCreateRole(string name, int roleID, ulong accountID)
    {
        if (string.IsNullOrEmpty(name))
        {
            GTItemHelper.ShowTip("名字不能为空");
            return;
        }
        RoleCreateReq req = new RoleCreateReq();
        req.Name      = name;
        req.AccountID = accountID;
        req.RoleType  = (uint)roleID;
        NetworkManager.Instance.Send(MessageID.MSG_ROLE_CREATE_REQ, req);
    }

    //取角色列表
    public void TryGetRoleList()
    {
        RoleListReq req = new RoleListReq();
        req.AccountID = 1;
        req.LoginCode = 0x111;
        NetworkManager.Instance.Send(MessageID.MSG_ROLE_LIST_REQ, req);
    }

    //登录角色
    public void TryEnterGame(UInt64 roleGUID)
    {
        RoleLoginReq req = new RoleLoginReq();
        req.AccountID = 1;
        req.RoleID = roleGUID;
        req.LoginCode = 0x111;
        NetworkManager.Instance.Send(MessageID.MSG_ROLE_LOGIN_REQ, req);
    }

    //进入副本场景或者主城场景
    public void TryEnterScene(UInt64 roleID, UInt32 CopyID, UInt32 ServerID)
    {
        EnterSceneReq req = new EnterSceneReq();
        req.RoleID = roleID;
        req.CopyID = CopyID;
        req.ServerID = ServerID;
        NetworkManager.Instance.Send(MessageID.MSG_ENTER_SCENE_REQ, req, (UInt64)CopyID, (UInt32)ServerID);
    }
}
