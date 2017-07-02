using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;
using Protocol;

public class PartnerService : GTSingleton<PartnerService>
{
    public void TryChangePartner(int pos, int id)
    {
        ReqChangePartner req = new ReqChangePartner();
        req.ID = id;
        req.Pos = pos;
        NetworkManager.Instance.Send<ReqChangePartner>(MessageID.MSG_REQ_CHANGE_PARTNER, req);
    }

    public void TryUpgradePartner(int id)
    {
        ReqUpgradePartner req = new ReqUpgradePartner();
        req.ID = id;
        NetworkManager.Instance.Send<ReqUpgradePartner>(MessageID.MSG_REQ_UPGRADE_PARTNER, req);
    }

    public void TryAdvancePartner(int id)
    {
        ReqAdvancePartner req = new ReqAdvancePartner();
        req.ID = id;
        NetworkManager.Instance.Send<ReqAdvancePartner>(MessageID.MSG_REQ_ADVANVE_PARTNER, req);
    }
}
