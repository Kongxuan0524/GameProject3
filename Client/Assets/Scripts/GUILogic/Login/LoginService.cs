using UnityEngine;
using System.Collections;
using Protocol;

public class LoginService : GTSingleton<LoginService>
{
    public void TryLoginGame(int serverID = 0)
    {
        ReqLoginGame req = new ReqLoginGame();
        req.ServerID = serverID;
        NetworkManager.Instance.Send(MessageID.MSG_REQ_LOGINSERVER, req);
    }

    public void TryCreateRole(string name, int id)
    {
        if (string.IsNullOrEmpty(name))
        {
            GTItemHelper.ShowTip("名字不能为空");
            return;
        }
        ReqCreateRole req = new ReqCreateRole();
        req.Name = name;
        req.ID = id;
        NetworkManager.Instance.Send(MessageID.MSG_REQ_CREATEROLE, req);
    }

    public void TryEnterGame(int roleId)
    {
        ReqEnterGame req = new ReqEnterGame();
        req.RoleID = roleId;
        NetworkManager.Instance.Send(MessageID.MSG_REQ_ENTERGAME, req);
    }
}
