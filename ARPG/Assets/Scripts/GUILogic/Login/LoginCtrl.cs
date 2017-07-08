using UnityEngine;
using System.Collections;
using System;
using Protocol;
using ProtoBuf;

public class LoginCtrl :  ICtrl
{
    public void AddListener()
    {
        NetworkManager.AddListener(MessageID.MSG_ACCOUNT_REG_ACK,      OnAck_AccountRegister);
        NetworkManager.AddListener(MessageID.MSG_ACCOUNT_LOGIN_ACK,    OnAck_AccountLogin);
        NetworkManager.AddListener(MessageID.MSG_SELECT_SERVER_ACK,    OnAck_SelectServer);
        NetworkManager.AddListener(MessageID.MSG_ROLE_LIST_ACK,        OnAck_GetRoleList);
        NetworkManager.AddListener(MessageID.MSG_SERVER_LIST_ACK,      OnAck_GetServerList);
        NetworkManager.AddListener(MessageID.MSG_ROLE_CREATE_ACK,      OnAck_CreateRole);
        NetworkManager.AddListener(MessageID.MSG_ROLE_LOGIN_ACK,       OnAck_RoleLoginAck);
        NetworkManager.AddListener(MessageID.MSG_NOTIFY_INTO_SCENE,    OnAck_NotifyIntoScene);
        NetworkManager.AddListener(MessageID.MSG_ENTER_SCENE_ACK,      OnAck_EnterScene);
    }

    public void DelListener()
    {
        NetworkManager.DelListener(MessageID.MSG_ACCOUNT_REG_ACK,      OnAck_AccountRegister);
        NetworkManager.DelListener(MessageID.MSG_ACCOUNT_LOGIN_ACK,    OnAck_AccountLogin);
        NetworkManager.DelListener(MessageID.MSG_SELECT_SERVER_ACK,    OnAck_SelectServer);
        NetworkManager.DelListener(MessageID.MSG_ROLE_LIST_ACK,        OnAck_GetRoleList);
        NetworkManager.DelListener(MessageID.MSG_SERVER_LIST_ACK,      OnAck_GetServerList);
        NetworkManager.DelListener(MessageID.MSG_ROLE_CREATE_ACK,      OnAck_CreateRole);
        NetworkManager.DelListener(MessageID.MSG_ROLE_LOGIN_ACK,       OnAck_RoleLoginAck);
        NetworkManager.DelListener(MessageID.MSG_NOTIFY_INTO_SCENE,    OnAck_NotifyIntoScene);
        NetworkManager.DelListener(MessageID.MSG_ENTER_SCENE_ACK,      OnAck_EnterScene);
    }

    private void OnAck_AccountRegister(MessageRecv obj)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        AccountRegAck ack = Serializer.Deserialize<AccountRegAck>(ms);
        GTEventCenter.FireEvent(GTEventID.TYPE_REGISTER_CALLBACK);
    }

    //响应账号登录成功
    private void OnAck_AccountLogin(MessageRecv obj)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        AccountLoginAck ack = Serializer.Deserialize<AccountLoginAck>(ms);

        ClientServerNode newNode = new ClientServerNode();
        newNode.SvrID = ack.LastSvrID;
        newNode.SvrName = ack.LastSvrName;
        LoginModule.Instance.SetCurrServer(newNode);
        LoginModule.Instance.LastAccountID = ack.AccountID;

        GTEventCenter.FireEvent(GTEventID.TYPE_ACCLOGIN_CALLBACK);
    }

  
    private void OnAck_GetServerList(MessageRecv obj)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ClientServerListAck ack = Serializer.Deserialize<ClientServerListAck>(ms);
        LoginModule.Instance.SetServerList(ack.SvrNode);

        GTEventCenter.FireEvent(GTEventID.TYPE_GETSERVERS_CALLBACK);
    }


    private void OnAck_SelectServer(MessageRecv obj)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        SelectServerAck ack = Serializer.Deserialize<SelectServerAck>(ms);
        GTEventCenter.FireEvent(GTEventID.TYPE_SELECTSERVER);
        if (GTGlobal.Along)
        {
            GTLauncher.Instance.LoadScene(GTSceneKey.SCENE_Role);
        }
        else
        {
            NetworkManager.Instance.Close();
            NetworkManager.Instance.ConnectGameServer(ack.ServerAddr, ack.ServerPort, () =>
            {
                RoleListReq req = new RoleListReq();
                req.AccountID = LoginModule.Instance.LastAccountID;
                req.LoginCode = ack.LoginCode;
                NetworkManager.Instance.Send(MessageID.MSG_ROLE_LIST_REQ, req);
            });
        }
    }

    private void OnAck_GetRoleList(MessageRecv obj)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        RoleListAck ack = Serializer.Deserialize<RoleListAck>(ms);
        for (int i = 0; i < ack.RoleList.Count; i++)
        {
            RoleItem item = ack.RoleList[i];
            XCharacter data = new XCharacter();
            data.Id = item.RoleType;
            data.GUID =  item.ID;
            data.Level = item.Level;
            data.Name = item.Name;
            DataDBSRole.Update(data.Id, data);
        }

        GTLauncher.Instance.LoadScene(GTSceneKey.SCENE_Role);
    }

    private void OnAck_CreateRole(MessageRecv obj)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        RoleCreateAck ack = Serializer.Deserialize<RoleCreateAck>(ms);

        XCharacter data = new XCharacter();
        data.Id = (int)ack.RoleType;
        data.GUID = ack.RoleID;
        data.Name = ack.Name;
        data.Level = 1;

        DataDBSRole.Insert(data.Id, data);
        GTEventCenter.FireEvent(GTEventID.TYPE_CREATEROLE_CALLBACK);
    }

    private void OnAck_RoleLoginAck(MessageRecv obj)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        RoleLoginAck ack = Serializer.Deserialize<RoleLoginAck>(ms);

        GTGlobal.CurPlayerID = (int)ack.RoleType;

        GTEventCenter.FireEvent(GTEventID.TYPE_ENTERGAME_CALLBACK);
        GTLauncher.Instance.LoadScene(GTGlobal.LAST_CITY_ID);
    }



    private void OnAck_NotifyIntoScene(MessageRecv obj)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        NotifyIntoScene ack = Serializer.Deserialize<NotifyIntoScene>(ms);
        EnterSceneReq req = new EnterSceneReq();
        req.RoleID = ack.RoleID;
        NetworkManager.Instance.Send(MessageID.MSG_ENTER_SCENE_REQ, req, (UInt64)ack.CopyID, (UInt32)ack.ServerID);
    }

    private void OnAck_EnterScene(MessageRecv obj)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        EnterSceneAck ack = Serializer.Deserialize<EnterSceneAck>(ms);


        GTEventCenter.FireEvent(GTEventID.TYPE_ENTERGAME_CALLBACK);
        GTLauncher.Instance.LoadScene(GTGlobal.LAST_CITY_ID);
    }
}
