using UnityEngine;
using System.Collections;
using Protocol;
using ProtoBuf;
using LitJson;
using System;
using System.Collections.Generic;

public class MessageCenter
{
    public void AddHandlers()
    {
        NetworkManager.AddListener(MessageID.MSG_ACCOUNT_LOGIN_REQ,    OnReq_Login);
        NetworkManager.AddListener(MessageID.MSG_SELECT_SERVER_REQ,    OnReq_LoginServer);
        NetworkManager.AddListener(MessageID.MSG_SERVER_LIST_REQ,      OnReq_GetServerList);
        NetworkManager.AddListener(MessageID.MSG_ROLE_LOGIN_REQ,       OnReq_EnterGame);
        NetworkManager.AddListener(MessageID.MSG_ACCOUNT_REG_REQ,      OnReq_Register);
        NetworkManager.AddListener(MessageID.MSG_ROLE_CREATE_REQ,      OnReq_CreateRole);

        NetworkManager.AddListener(MessageID.MSG_REQ_SORTBAG,          OnReq_SortBag);
        NetworkManager.AddListener(MessageID.MSG_REQ_USEITEM,          OnReq_UseItem);
        NetworkManager.AddListener(MessageID.MSG_REQ_DRESS_EQUIP,      OnReq_DressEquip);
        NetworkManager.AddListener(MessageID.MSG_REQ_UNLOAD_EQUIP,     OnReq_UnloadEquip);
        NetworkManager.AddListener(MessageID.MSG_REQ_DRESS_GEM,        OnReq_DressGem);
        NetworkManager.AddListener(MessageID.MSG_REQ_UNLOAD_GEM,       OnReq_UnloadGem);

        NetworkManager.AddListener(MessageID.MSG_REQ_COMPOSE_CHIP,     OnReq_ComposeChip);
        NetworkManager.AddListener(MessageID.MSG_REQ_STRENGTHEN_EQUIP, OnReq_StrengthEquip);
        NetworkManager.AddListener(MessageID.MSG_REQ_ADVANCE_EQUIP,    OnReq_AdvanceEquip);
        NetworkManager.AddListener(MessageID.MSG_REQ_UPSTAR_EQUIP,     OnReq_UpStarEquip);
        NetworkManager.AddListener(MessageID.MSG_REQ_STRENGTHEN_GEM,   OnReq_StrengthGem);

        NetworkManager.AddListener(MessageID.MSG_REQ_GET_CHAPTERAWARD, OnReq_GetChapterAward);
        NetworkManager.AddListener(MessageID.MSG_REQ_BATTLE_CHECK,     OnReq_BattleCheck);
        NetworkManager.AddListener(MessageID.MSG_REQ_PASSCOPY,         OnReq_PassCopy);
        NetworkManager.AddListener(MessageID.MSG_REQ_SETMOUNT,         OnReq_SetMount);

        NetworkManager.AddListener(MessageID.MSG_REQ_CHARGE_RELICS,    OnReq_ChargeRelics);
        NetworkManager.AddListener(MessageID.MSG_REQ_UPGRADE_RELICS,   OnReq_UpgradeRelics);
        NetworkManager.AddListener(MessageID.MSG_REQ_BATTLE_RELICS,    OnReq_BattleRelics);
        NetworkManager.AddListener(MessageID.MSG_REQ_UNLOAD_RELICS,    OnReq_UnloadRelics);

        NetworkManager.AddListener(MessageID.MSG_REQ_BUY_STORE,        OnReq_BuyStore);

        NetworkManager.AddListener(MessageID.MSG_REQ_UPGRADE_PET,      OnReq_UpgradePet);
        NetworkManager.AddListener(MessageID.MSG_REQ_BATTLE_PET,       OnReq_BattlePet);
        NetworkManager.AddListener(MessageID.MSG_REQ_UNLOAD_PET,       OnReq_UnloadPet);

        NetworkManager.AddListener(MessageID.MSG_REQ_CHANGE_PARTNER,   OnReq_ChangePartner);
        NetworkManager.AddListener(MessageID.MSG_REQ_ADVANVE_PARTNER,  OnReq_AdvancePartner);
        NetworkManager.AddListener(MessageID.MSG_REQ_UPGRADE_PARTNER,  OnReq_UpgradePartner);

        NetworkManager.AddListener(MessageID.MSG_REQ_SUBMIT_TASK,      OnReq_SubmitTask);
        NetworkManager.AddListener(MessageID.MSG_REQ_ADDHERO_EXP,      OnReq_AddHeroExp);
    }

    public void DelHandlers()
    {
        NetworkManager.DelListener(MessageID.MSG_ACCOUNT_LOGIN_REQ,    OnReq_Login);
        NetworkManager.DelListener(MessageID.MSG_SELECT_SERVER_REQ,    OnReq_LoginServer);
        NetworkManager.DelListener(MessageID.MSG_SERVER_LIST_REQ,      OnReq_GetServerList);
        NetworkManager.DelListener(MessageID.MSG_ROLE_LOGIN_REQ,       OnReq_EnterGame);
        NetworkManager.DelListener(MessageID.MSG_ACCOUNT_REG_REQ,      OnReq_Register);
        NetworkManager.DelListener(MessageID.MSG_ROLE_CREATE_REQ,      OnReq_CreateRole);

        NetworkManager.DelListener(MessageID.MSG_REQ_SORTBAG,          OnReq_SortBag);
        NetworkManager.DelListener(MessageID.MSG_REQ_USEITEM,          OnReq_UseItem);
        NetworkManager.DelListener(MessageID.MSG_REQ_DRESS_EQUIP,      OnReq_DressEquip);
        NetworkManager.DelListener(MessageID.MSG_REQ_UNLOAD_EQUIP,     OnReq_UnloadEquip);
        NetworkManager.DelListener(MessageID.MSG_REQ_DRESS_GEM,        OnReq_DressGem);
        NetworkManager.DelListener(MessageID.MSG_REQ_UNLOAD_GEM,       OnReq_UnloadGem);

        NetworkManager.DelListener(MessageID.MSG_REQ_COMPOSE_CHIP,     OnReq_ComposeChip);
        NetworkManager.DelListener(MessageID.MSG_REQ_STRENGTHEN_EQUIP, OnReq_StrengthEquip);
        NetworkManager.DelListener(MessageID.MSG_REQ_ADVANCE_EQUIP,    OnReq_AdvanceEquip);
        NetworkManager.DelListener(MessageID.MSG_REQ_UPSTAR_EQUIP,     OnReq_UpStarEquip);
        NetworkManager.DelListener(MessageID.MSG_REQ_STRENGTHEN_GEM,   OnReq_StrengthGem);

        NetworkManager.DelListener(MessageID.MSG_REQ_GET_CHAPTERAWARD, OnReq_GetChapterAward);
        NetworkManager.DelListener(MessageID.MSG_REQ_BATTLE_CHECK,     OnReq_BattleCheck);
        NetworkManager.DelListener(MessageID.MSG_REQ_PASSCOPY,         OnReq_PassCopy);
        NetworkManager.DelListener(MessageID.MSG_REQ_SETMOUNT,         OnReq_SetMount);

        NetworkManager.DelListener(MessageID.MSG_REQ_CHARGE_RELICS,    OnReq_ChargeRelics);
        NetworkManager.DelListener(MessageID.MSG_REQ_UPGRADE_RELICS,   OnReq_UpgradeRelics);
        NetworkManager.DelListener(MessageID.MSG_REQ_BATTLE_RELICS,    OnReq_BattleRelics);
        NetworkManager.DelListener(MessageID.MSG_REQ_UNLOAD_RELICS,    OnReq_UnloadRelics);

        NetworkManager.DelListener(MessageID.MSG_REQ_BUY_STORE,        OnReq_BuyStore);

        NetworkManager.DelListener(MessageID.MSG_REQ_UPGRADE_PET,      OnReq_UpgradePet);
        NetworkManager.DelListener(MessageID.MSG_REQ_BATTLE_PET,       OnReq_BattlePet);
        NetworkManager.DelListener(MessageID.MSG_REQ_UNLOAD_PET,       OnReq_UnloadPet);

        NetworkManager.DelListener(MessageID.MSG_REQ_CHANGE_PARTNER,   OnReq_ChangePartner);
        NetworkManager.DelListener(MessageID.MSG_REQ_ADVANVE_PARTNER,  OnReq_AdvancePartner);
        NetworkManager.DelListener(MessageID.MSG_REQ_UPGRADE_PARTNER,  OnReq_UpgradePartner);

        NetworkManager.DelListener(MessageID.MSG_REQ_SUBMIT_TASK,      OnReq_SubmitTask);
        NetworkManager.DelListener(MessageID.MSG_REQ_ADDHERO_EXP,      OnReq_AddHeroExp);
    }


    private void OnReq_Login(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        AccountLoginReq req = Serializer.Deserialize<AccountLoginReq>(ms);
        AccountLoginAck ack = new AccountLoginAck();
        NetworkManager.Instance.Send(MessageID.MSG_ACCOUNT_LOGIN_ACK, ack, 0, 0);
    }

    private void OnReq_LoginServer(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        SelectServerReq req = Serializer.Deserialize<SelectServerReq>(ms);
        SelectServerAck ack = new SelectServerAck();
        NetworkManager.Instance.Send(MessageID.MSG_SELECT_SERVER_ACK, ack, 0, 0);
    }

    private void OnReq_GetServerList(MessageRecv obj)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ClientServerListReq req = Serializer.Deserialize<ClientServerListReq>(ms);
        ClientServerListAck ack = new ClientServerListAck();
        NetworkManager.Instance.Send(MessageID.MSG_SERVER_LIST_ACK, ack, 0, 0);
    }

    private void OnReq_Register(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        AccountRegReq req = Serializer.Deserialize<AccountRegReq>(ms);
        AccountRegAck ack = new Protocol.AccountRegAck();
        NetworkManager.Instance.Send(MessageID.MSG_ACCOUNT_REG_ACK, ack, 0, 0);
    }

    private void OnReq_CreateRole(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        RoleCreateReq req = Serializer.Deserialize<RoleCreateReq>(ms);

        RoleCreateAck ack = new RoleCreateAck();
        ack.RoleID = GTGUID.NewGUID();
        ack.Name = req.Name;
        ack.RoleType = req.RoleType;
        ack.AccountID = req.AccountID;
        NetworkManager.Instance.Send(MessageID.MSG_ROLE_CREATE_ACK, ack, 0, 0);
    }

    private void OnReq_EnterGame(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        RoleLoginReq req = Serializer.Deserialize<RoleLoginReq>(ms);
        RoleLoginAck ack = new RoleLoginAck();
        foreach(var current in DataDBSRole.Dict)
        {
            XCharacter c = current.Value;
            if(c.GUID == req.RoleID)
            {
                ack.RoleID = c.GUID;
                ack.Name = c.Name;
                ack.RoleType = (uint)c.Id;
                ack.Level = c.Level;
                ack.Exp = (ulong)c.CurExp;
                NetworkManager.Instance.Send(MessageID.MSG_ROLE_LOGIN_ACK, ack, 0, 0);
                break;
            }
        }
    }

    private void OnReq_SortBag(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqSortBag req = Serializer.Deserialize<ReqSortBag>(ms);

        AckSortBag ack = new AckSortBag();
        ack.BagType = req.BagType;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_SORTBAG, ack, 0, 0);
    }

    private void OnReq_UnloadGem(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqUnloadGem req = Serializer.Deserialize<ReqUnloadGem>(ms);

        AckUnloadGem ack = new AckUnloadGem();
        ack.TarPos = req.TarPos;
        ack.Index  = req.Index;
        ack.NewPos = DataManager.Instance.GetNewPos(EBagType.GEM);
        NetworkManager.Instance.Send(MessageID.MSG_ACK_UNLOAD_GEM, ack, 0, 0);
    }

    private void OnReq_DressGem(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqDressGem req = Serializer.Deserialize<ReqDressGem>(ms);

        XItem bagGem = DataDBSBagGem.GetDataById(req.SrcPos);
        DGem cfg = ReadCfgGem.GetDataById(bagGem.Id);
        AckDressGem ack = new AckDressGem();
        ack.SrcPos = req.SrcPos;
        ack.Index  = req.Index;
        ack.TarPos = cfg.Pos;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_DRESS_GEM, ack, 0, 0);
    }

    private void OnReq_UnloadEquip(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqUnloadEquip req = Serializer.Deserialize<ReqUnloadEquip>(ms);

        AckUnloadEquip ack = new AckUnloadEquip();
        ack.TarPos = req.TarPos;
        ack.NewPos = DataManager.Instance.GetNewPos(EBagType.ITEM);
        NetworkManager.Instance.Send(MessageID.MSG_ACK_UNLOAD_EQUIP, ack, 0, 0);
    }

    private void OnReq_DressEquip(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqDressEquip req = Serializer.Deserialize<ReqDressEquip>(ms);

        AckDressEquip ack = new AckDressEquip();
        XItem bagEquip = DataDBSBagItem.GetDataById(req.SrcPos);
        DEquip equipDB = ReadCfgEquip.GetDataById(bagEquip.Id);
        ack.SrcPos = req.SrcPos;
        ack.TarPos = equipDB.Pos;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_DRESS_EQUIP, ack, 0, 0);
    }

    private void OnReq_UseItem(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqUseItem req = Serializer.Deserialize<ReqUseItem>(ms);

        AckUseItem ack = new AckUseItem();
        ack.Num = req.Num;
        ack.Pos = req.Pos;
        ack.PosType = req.PosType;
        ack.ID = req.ID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_USEITEM, ack, 0, 0);
    }

    private void OnReq_ComposeChip(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqComposeChip req = Serializer.Deserialize<ReqComposeChip>(ms);

        AckComposeChip ack = new AckComposeChip();
        ack.SrcPos = req.SrcPos;
        ack.Num = req.Num;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_COMPOSE_CHIP, ack, 0, 0);
    }

    private void OnReq_AddHeroExp(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqAddPlayerExp req = Serializer.Deserialize<ReqAddPlayerExp>(ms);

        AckAddPlayerExp ack = new AckAddPlayerExp();
        ack.Exp = req.Exp;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_ADDHERO_EXP, ack, 0, 0);
    }

    private void OnReq_BuyStore(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqBuyStore req = Serializer.Deserialize<ReqBuyStore>(ms);

        AckBuyStore ack = new AckBuyStore();
        ack.StoreID     = req.StoreID;
        ack.Num         = req.Num;
        ack.StoreType   = req.StoreType;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_BUY_STORE, ack, 0, 0);
    }

    private void OnReq_StrengthGem(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqStrengthGem req = Serializer.Deserialize<ReqStrengthGem>(ms);

        AckStrengthGem ack = new AckStrengthGem();
        ack.TarGem = req.TarGem;
        ack.UseItems.AddRange(req.UseItems);
        NetworkManager.Instance.Send(MessageID.MSG_ACK_STRENGTHEN_GEM, ack, 0, 0);
    }

    private void OnReq_UpStarEquip(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqUpStarEquip req = Serializer.Deserialize<ReqUpStarEquip>(ms);

        AckUpStarEquip ack = new AckUpStarEquip();
        ack.TarEquip = req.TarEquip;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_UPSTAR_EQUIP, ack, 0, 0);
    }

    private void OnReq_AdvanceEquip(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqAdvanceEquip req = Serializer.Deserialize<ReqAdvanceEquip>(ms);

        DEquip        cfg = ReadCfgEquip.GetDataById(req.TarEquip.Id);
        int advanceID = cfg.Quality * 1000 + req.TarEquip.AdvanceLevel + 1;
        DEquipAdvanceCost db = ReadCfgEquipAdvanceCost.GetDataById(advanceID);

        AckAdvanceEquip ack = new AckAdvanceEquip();
        ack.UseItems.AddRange(req.UseItems);
        ack.TarEquip = req.TarEquip;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_ADVANCE_EQUIP, ack, 0, 0);
    }

    private void OnReq_StrengthEquip(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqStrengthEquip req = Serializer.Deserialize<ReqStrengthEquip>(ms);

        AckStrengthEquip ack = new AckStrengthEquip();
        ack.TarEquip = req.TarEquip;
        ack.UseItems.AddRange(req.UseItems);
        NetworkManager.Instance.Send(MessageID.MSG_ACK_STRENGTHEN_EQUIP, ack, 0, 0);
    }

    private void OnReq_SubmitTask(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqSubmitTask req = Serializer.Deserialize<ReqSubmitTask>(ms);

        AckSubmitTask ack = new AckSubmitTask();
        ack.ID   = req.ID;
        ack.Step = req.Step;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_SUBMIT_TASK, ack, 0, 0);
    }

    private void OnReq_UpgradePartner(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqUpgradePartner req = Serializer.Deserialize<ReqUpgradePartner>(ms);

        AckUpgradePartner ack = new AckUpgradePartner();
        ack.ID = req.ID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_UPGRADE_PARTNER, ack, 0, 0);
    }

    private void OnReq_AdvancePartner(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqAdvancePartner req = Serializer.Deserialize<ReqAdvancePartner>(ms);

        AckAdvancePartner ack = new AckAdvancePartner();
        ack.ID = req.ID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_ADVANVE_PARTNER, ack, 0, 0);
    }

    private void OnReq_ChangePartner(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqChangePartner req = Serializer.Deserialize<ReqChangePartner>(ms);

        AckChangePartner ack = new AckChangePartner();
        ack.ID = req.ID;
        ack.Pos = req.Pos;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_CHANGE_PARTNER, ack, 0, 0);
    }

    private void OnReq_UnloadPet(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqUnloadPet req = Serializer.Deserialize<ReqUnloadPet>(ms);

        AckUnloadPet ack = new AckUnloadPet();
        ack.ID = req.ID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_UNLOAD_PET, ack, 0, 0);
    }

    private void OnReq_BattlePet(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqBattlePet req = Serializer.Deserialize<ReqBattlePet>(ms);

        AckBattlePet ack = new AckBattlePet();
        ack.ID = req.ID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_BATTLE_PET, ack, 0, 0);
    }

    private void OnReq_UpgradePet(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqUpgradePet req = Serializer.Deserialize<ReqUpgradePet>(ms);

        AckUpgradePet ack = new AckUpgradePet();
        ack.ID = req.ID;
        ack.UseItems.AddRange(req.UseItems);
        NetworkManager.Instance.Send(MessageID.MSG_ACK_UPGRADE_PET, ack, 0, 0);
    }

    private void OnReq_UnloadRelics(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqUnloadRelics req = Serializer.Deserialize<ReqUnloadRelics>(ms);

        AckUnloadRelics ack = new AckUnloadRelics();
        ack.RelicsID = req.RelicsID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_UNLOAD_RELICS, ack, 0, 0);
    }

    private void OnReq_BattleRelics(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqBattleRelics req = Serializer.Deserialize<ReqBattleRelics>(ms);

        AckBattleRelics ack = new AckBattleRelics();
        ack.RelicsID = req.RelicsID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_BATTLE_RELICS, ack, 0, 0);
    }

    private void OnReq_UpgradeRelics(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqUpgradeRelics req = Serializer.Deserialize<ReqUpgradeRelics>(ms);

        AckUpgradeRelics ack = new AckUpgradeRelics();
        ack.RelicsID = req.RelicsID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_UPGRADE_RELICS, ack, 0, 0);
    }

    private void OnReq_ChargeRelics(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqChargeRelics req = Serializer.Deserialize<ReqChargeRelics>(ms);

        AckChargeRelics ack = new AckChargeRelics();
        ack.RelicsID = req.RelicsID;
        ack.Index    = req.Index;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_CHARGE_RELICS, ack, 0, 0);
    }

    private void OnReq_SetMount(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqSetMount req = Serializer.Deserialize<ReqSetMount>(ms);

        AckSetMount ack = new AckSetMount();
        ack.MountID = req.MountID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_SETMOUNT, ack, 0, 0);
    }

    private void OnReq_PassCopy(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqPassCopy req = Serializer.Deserialize<ReqPassCopy>(ms);

        AckPassCopy ack = new AckPassCopy();
        ack.CopyType = req.CopyType;
        ack.CopyID = req.CopyID;
        ack.Chapter = req.Chapter;
        ack.StarNum = req.StarNum;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_PASSCOPY, ack, 0, 0);
    }

    private void OnReq_BattleCheck(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqBattleCheck req = Serializer.Deserialize<ReqBattleCheck>(ms);

        AckBattleCheck ack = new AckBattleCheck();
        ack.CopyType = req.CopyType;
        ack.Chapter = req.Chapter;
        ack.CopyID = req.CopyID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_BATTLE_CHECK, ack, 0, 0);
    }

    private void OnReq_GetChapterAward(MessageRecv obj )
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Data);
        ReqGetChapterAward req = Serializer.Deserialize<ReqGetChapterAward>(ms);

        AckGetChapterAward ack = new AckGetChapterAward();
        ack.CopyType = req.CopyType;
        ack.Chapter = req.Chapter;
        ack.Index = req.Index;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_GET_CHAPTERAWARD, ack, 0, 0);
    }
}
