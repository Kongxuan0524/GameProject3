using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Protocol;

public class LoginModule : GTSingleton<LoginModule>
{
    private List<XServer> m_ServerList = new List<XServer>()
    {
        new XServer() { ID = 1, Area = 1, Name = "洪荒之力", State = 1, Addr = "127.0.0.1:4055" },
        new XServer() { ID = 1, Area = 1, Name = "狂风暴雨", State = 1, Addr = "127.0.0.1:4055" },
        new XServer() { ID = 1, Area = 1, Name = "血灵之力", State = 1, Addr = "127.0.0.1:4055" },
        new XServer() { ID = 1, Area = 1, Name = "屠魔峡谷", State = 1, Addr = "127.0.0.1:4055" },
        new XServer() { ID = 1, Area = 1, Name = "神通广大", State = 1, Addr = "127.0.0.1:4055" },
        new XServer() { ID = 1, Area = 1, Name = "法力无边", State = 1, Addr = "127.0.0.1:4055" },
        new XServer() { ID = 1, Area = 1, Name = "妖神出世", State = 1, Addr = "127.0.0.1:4055" },
        new XServer() { ID = 1, Area = 1, Name = "熔岩之心", State = 1, Addr = "127.0.0.1:4055" }
    };

    private string        m_LastUsername = string.Empty;
    private string        m_LastPassword = string.Empty;
    private XServer       m_CurrServer;

    public LoginModule()
    {
        m_LastUsername = PlayerPrefs.GetString("Username", string.Empty);
        m_LastPassword = PlayerPrefs.GetString("Password", string.Empty);
    }

    public List<XServer> GetServerList()
    {
        return m_ServerList;
    }

    public void          SetServerList(List<XServer> value)
    {
        m_ServerList = value;
    }

    public XServer       GetCurrServer()
    {
        return m_CurrServer;
    }

    public void          SetCurrServer(XServer server)
    {
        m_CurrServer = server;
    }

    public string        LastUsername
    {
        get { return m_LastUsername; }
        set
        {
            m_LastUsername = value;
            PlayerPrefs.SetString("Username", m_LastUsername);
        }
    }

    public string        LastPassword
    {
        get { return m_LastPassword; }
        set
        {
            m_LastPassword = value;
            PlayerPrefs.SetString("Password", m_LastPassword);
        }
    }
}
