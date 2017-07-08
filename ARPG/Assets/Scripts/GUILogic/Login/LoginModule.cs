using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Protocol;
using System;

public class LoginModule : GTSingleton<LoginModule>
{
    private List<ClientServerNode> m_ServerList = new List<ClientServerNode>()
    {
        new ClientServerNode() { SvrID = 1, SvrDefault = 1, SvrName = "洪荒之力", SvrState = 1, SvrOpenTime =100},
        new ClientServerNode() { SvrID = 1, SvrDefault = 1, SvrName = "狂风暴雨", SvrState = 1, SvrOpenTime =100},
        new ClientServerNode() { SvrID = 1, SvrDefault = 1, SvrName = "血灵之力", SvrState = 1, SvrOpenTime =100},
        new ClientServerNode() { SvrID = 1, SvrDefault = 1, SvrName = "屠魔峡谷", SvrState = 1, SvrOpenTime =100},
        new ClientServerNode() { SvrID = 1, SvrDefault = 1, SvrName = "神通广大", SvrState = 1, SvrOpenTime =100},
        new ClientServerNode() { SvrID = 1, SvrDefault = 1, SvrName = "法力无边", SvrState = 1, SvrOpenTime =100},
        new ClientServerNode() { SvrID = 1, SvrDefault = 1, SvrName = "妖神出世", SvrState = 1, SvrOpenTime =100},
        new ClientServerNode() { SvrID = 1, SvrDefault = 1, SvrName = "熔岩之心", SvrState = 1, SvrOpenTime =100}
    };

    private string           m_LastUsername = string.Empty;
    private string           m_LastPassword = string.Empty;
    private XServer          m_CurrServer;

    public LoginModule()
    {
        m_LastUsername = PlayerPrefs.GetString("Username", string.Empty);
        m_LastPassword = PlayerPrefs.GetString("Password", string.Empty);
    }

    public List<ClientServerNode> GetServerList()
    {
        return m_ServerList;
    }

    public void                   SetServerList(List<ClientServerNode> value)
    {
        m_ServerList = value;
    }

    public XServer                GetCurrServer()
    {
        return m_CurrServer;
    }

    public void                   SetCurrServer(ClientServerNode server)
    {
        m_CurrServer = new XServer();
        m_CurrServer.ID = server.SvrID;
        m_CurrServer.Name = server.SvrName;
        m_CurrServer.State =(int) server.SvrState;
    }

    public string                 LastUsername
    {
        get { return m_LastUsername; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            m_LastUsername = value;
            PlayerPrefs.SetString("Username", m_LastUsername);
        }
    }

    public string                 LastPassword
    {
        get { return m_LastPassword; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            m_LastPassword = value;
            PlayerPrefs.SetString("Password", m_LastPassword);
        }
    }

    public UInt64                 LastAccountID
    {
        get; set;
    }
}
