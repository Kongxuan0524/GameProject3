using UnityEngine;
using System.Collections;
using System;
using Protocol;
using ProtoBuf;

public class LoginCtrl :  ICtrl
{
    public void AddListener()
    {
        NetworkManager.AddListener(MessageID.MSG_ACK_REGISTER,      OnAck_Register);
        NetworkManager.AddListener(MessageID.MSG_ACK_LOGIN,         OnAck_Login);
        NetworkManager.AddListener(MessageID.MSG_ACK_LOGINSERVER,   OnAck_LoginGame);
        NetworkManager.AddListener(MessageID.MSG_ACK_GETSERVERLIST, OnAck_GetServerList);
        NetworkManager.AddListener(MessageID.MSG_ACK_ENTERGAME,     OnAck_EnterGame);
        NetworkManager.AddListener(MessageID.MSG_ACK_CREATEROLE,    OnAck_CreateRole);
    }

    public void DelListener()
    {
        NetworkManager.DelListener(MessageID.MSG_ACK_REGISTER, OnAck_Register);
        NetworkManager.DelListener(MessageID.MSG_ACK_LOGIN, OnAck_Login);
        NetworkManager.DelListener(MessageID.MSG_ACK_LOGINSERVER, OnAck_LoginGame);
        NetworkManager.DelListener(MessageID.MSG_ACK_GETSERVERLIST, OnAck_GetServerList);
        NetworkManager.DelListener(MessageID.MSG_ACK_ENTERGAME, OnAck_EnterGame);
        NetworkManager.DelListener(MessageID.MSG_ACK_CREATEROLE, OnAck_CreateRole);
    }

    private void OnAck_CreateRole(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        AckCreateRole ack = Serializer.Deserialize<AckCreateRole>(ms);
        DataDBSRole.Insert(ack.Player.Id, ack.Player);

        GTEventCenter.FireEvent(GTEventID.TYPE_CREATEROLE_CALLBACK);
    }

    private void OnAck_EnterGame(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        AckEnterGame ack = Serializer.Deserialize<AckEnterGame>(ms);
        GTLauncher.CurPlayerID = ack.Player.Id;

        GTEventCenter.FireEvent(GTEventID.TYPE_ENTERGAME_CALLBACK);
        GTLauncher.Instance.LoadScene(GTLauncher.LAST_CITY_ID);
    }

    private void OnAck_GetServerList(MessageRecv obj, MessageRetCode retCode)
    {

    }

    private void OnAck_LoginGame(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        AckLoginGame ack = Serializer.Deserialize<AckLoginGame>(ms);

        GTEventCenter.FireEvent(GTEventID.TYPE_LOGINGAME_CALLBACK);
        GTLauncher.Instance.LoadScene(GTSceneKey.SCENE_Role);
    }

    private void OnAck_Login(MessageRecv obj, MessageRetCode retCode)
    {
        GTEventCenter.FireEvent(GTEventID.TYPE_LOGIN_CALLBACK);
    }

    private void OnAck_Register(MessageRecv obj, MessageRetCode retCode)
    {
        GTEventCenter.FireEvent(GTEventID.TYPE_REGISTER_CALLBACK);
    }
}
