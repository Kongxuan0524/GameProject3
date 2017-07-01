using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Protocol;

public class BagService: GTSingleton<BagService>
{
    public void TrySortBag(EBagType bagType)
    {
        ReqSortBag req = new ReqSortBag();
        req.BagType = (int)bagType;
        NetworkManager.Instance.Send<ReqSortBag>(MessageID.MSG_REQ_SORTBAG, req);
    }

    public void TryUseItemByPos(int pos, int num)
    {
        XItem item = DataDBSBagItem.GetDataById(pos);
        if (item.Num < num)
        {
            GTItemHelper.ShowTip("物品不足");
            return;
        }
        DItem itemDB = ReadCfgItem.GetDataById(item.Id);
        if (itemDB.ItemType == EItemType.BOX)
        {
            if (itemDB.Data1 != 0)
            {
                if (DataManager.Instance.GetItemCountById(itemDB.Data1) < num)
                {
                    GTItemHelper.ShowTip("需要足够的钥匙");
                    return;
                }
            }
            DAward awardDB = ReadCfgAward.GetDataById(itemDB.Data2);
            if(GTItemHelper.CheckBagFull(awardDB.MaxDropNum))
            {
                return;
            }
        }

        ReqUseItem req = new ReqUseItem();
        req.Pos = pos;
        req.Num = num;
        req.PosType = (int)EPosType.BagItem;
        req.ID = item.Id;
        NetworkManager.Instance.Send<ReqUseItem>(MessageID.MSG_REQ_USEITEM, req);
    }

    public void TryDressEquip(int srcPos)
    {
        ReqDressEquip req = new ReqDressEquip();
        req.SrcPos = srcPos;
        NetworkManager.Instance.Send<ReqDressEquip>(MessageID.MSG_REQ_DRESS_EQUIP, req);
    }

    public void TryUnloadEquip(int tarPos)
    {
        if (GTItemHelper.CheckBagFull(1, EBagType.ITEM))
        {
            return;
        }
        ReqUnloadEquip req = new ReqUnloadEquip();
        req.TarPos = tarPos;
        NetworkManager.Instance.Send<ReqUnloadEquip>(MessageID.MSG_REQ_UNLOAD_EQUIP, req);
    }

    public void TryDressGem(int index, int srcPos)
    {
        ReqDressGem req = new ReqDressGem();
        req.SrcPos = srcPos;
        req.Index = index;
        NetworkManager.Instance.Send<ReqDressGem>(MessageID.MSG_REQ_DRESS_GEM, req);
    }

    public void TryUnloadGem(int index, int tarPos)
    {
        if (GTItemHelper.CheckBagFull(1, EBagType.GEM))
        {
            return;
        }

        ReqUnloadGem req = new ReqUnloadGem();
        req.TarPos = tarPos;
        req.Index = index;
        NetworkManager.Instance.Send<ReqUnloadGem>(MessageID.MSG_REQ_UNLOAD_GEM, req);
    }

    public void TryComposeChip(int srcPos)
    {
        XItem   item   = DataDBSBagChip.Dict[srcPos];
        DItem itemDB = ReadCfgItem.GetDataById(item.Id);
        int composeNum = item.Num / itemDB.Data1;
        if (GTItemHelper.CheckBagFull(composeNum, EBagType.GEM))
        {
            return;
        }
        ReqComposeChip req = new ReqComposeChip();
        req.SrcPos = srcPos;
        req.Num    = composeNum;
        NetworkManager.Instance.Send<ReqComposeChip>(MessageID.MSG_REQ_COMPOSE_CHIP, req);
    }

    public void TryOneKeyToDressGem(int index)
    {
        ReqOneKeyToDressGem req = new ReqOneKeyToDressGem();
        req.Index = index;
        NetworkManager.Instance.Send<ReqOneKeyToDressGem>(MessageID.MSG_REQ_ONEKEYTODRESSGEM, req);
    }

    public void TryOneKeyToUnloadGem(int index)
    {
        ReqOneKeyToUnloadGem req = new ReqOneKeyToUnloadGem();
        req.Index = index;
        NetworkManager.Instance.Send<ReqOneKeyToUnloadGem>(MessageID.MSG_REQ_ONEKEYTOUNLOADGEM, req);
    }
}
