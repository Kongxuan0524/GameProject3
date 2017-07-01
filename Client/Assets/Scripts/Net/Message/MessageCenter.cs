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
        NetworkManager.AddListener(MessageID.MSG_REQ_LOGIN,            OnReq_Login);
        NetworkManager.AddListener(MessageID.MSG_REQ_LOGINSERVER,      OnReq_LoginServer);
        NetworkManager.AddListener(MessageID.MSG_REQ_GETSERVERLIST,    OnReq_GetServerList);
        NetworkManager.AddListener(MessageID.MSG_REQ_ENTERGAME,        OnReq_EnterGame);
        NetworkManager.AddListener(MessageID.MSG_REQ_REGISTER,         OnReq_Register);
        NetworkManager.AddListener(MessageID.MSG_REQ_CREATEROLE,       OnReq_CreateRole);

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

    private void OnReq_Login(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqLogin req = Serializer.Deserialize<ReqLogin>(ms);
        AckLogin ack = new Protocol.AckLogin();
        ack.AuthToken = "1234567890";
        ack.GameServer = new XServer
        {
            ID    = 1,
            State = 2,
            Addr  = "127.0.0.1:3035",
            Area  = 1,
            Name  = "阴阳之沟"
        };
        NetworkManager.Instance.Send(MessageID.MSG_ACK_LOGIN, ack);
    }

    private void OnReq_LoginServer(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqLoginGame req = Serializer.Deserialize<ReqLoginGame>(ms);
        AckLoginGame ack = new AckLoginGame();
        NetworkManager.Instance.Send(MessageID.MSG_ACK_LOGINSERVER, ack);
    }

    private void OnReq_GetServerList(MessageRecv obj,MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqGetServers req = Serializer.Deserialize<ReqGetServers>(ms);
        AckGetServers ack = new AckGetServers();
        NetworkManager.Instance.Send(MessageID.MSG_ACK_GETSERVERLIST, ack);
    }

    private void OnReq_Register(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqRegister req = Serializer.Deserialize<ReqRegister>(ms);
        AckRegister ack = new AckRegister();
        NetworkManager.Instance.Send(MessageID.MSG_ACK_REGISTER, ack);
    }

    private void OnReq_CreateRole(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqCreateRole req = Serializer.Deserialize<ReqCreateRole>(ms);

        AckCreateRole ack = new AckCreateRole();
        ack.Player = DataFactory.CreatePlayer(req.ID, req.Name);
        NetworkManager.Instance.Send(MessageID.MSG_ACK_CREATEROLE, ack);
    }

    private void OnReq_EnterGame(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqEnterGame req = Serializer.Deserialize<ReqEnterGame>(ms);
        
        AckEnterGame ack = new AckEnterGame();
        ack.Player = DataDBSRole.GetDataById(req.RoleID);
        NetworkManager.Instance.Send(MessageID.MSG_ACK_ENTERGAME, ack);
    }

    private void OnReq_SortBag(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqSortBag req = Serializer.Deserialize<ReqSortBag>(ms);

        AckSortBag ack = new AckSortBag();
        ack.BagType = req.BagType;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_SORTBAG, ack);
    }

    private void OnReq_UnloadGem(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqUnloadGem req = Serializer.Deserialize<ReqUnloadGem>(ms);

        AckUnloadGem ack = new AckUnloadGem();
        ack.TarPos = req.TarPos;
        ack.Index  = req.Index;
        ack.NewPos = DataManager.Instance.GetNewPos(EBagType.GEM);
        NetworkManager.Instance.Send(MessageID.MSG_ACK_UNLOAD_GEM, ack);
    }

    private void OnReq_DressGem(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqDressGem req = Serializer.Deserialize<ReqDressGem>(ms);

        XItem bagGem = DataDBSBagGem.GetDataById(req.SrcPos);
        DGem cfg = ReadCfgGem.GetDataById(bagGem.Id);
        AckDressGem ack = new AckDressGem();
        ack.SrcPos = req.SrcPos;
        ack.Index  = req.Index;
        ack.TarPos = cfg.Pos;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_DRESS_GEM, ack);
    }

    private void OnReq_UnloadEquip(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqUnloadEquip req = Serializer.Deserialize<ReqUnloadEquip>(ms);

        AckUnloadEquip ack = new AckUnloadEquip();
        ack.TarPos = req.TarPos;
        ack.NewPos = DataManager.Instance.GetNewPos(EBagType.ITEM);
        NetworkManager.Instance.Send(MessageID.MSG_ACK_UNLOAD_EQUIP, ack);
    }

    private void OnReq_DressEquip(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqDressEquip req = Serializer.Deserialize<ReqDressEquip>(ms);

        AckDressEquip ack = new AckDressEquip();
        XItem bagEquip = DataDBSBagItem.GetDataById(req.SrcPos);
        DEquip equipDB = ReadCfgEquip.GetDataById(bagEquip.Id);
        ack.SrcPos = req.SrcPos;
        ack.TarPos = equipDB.Pos;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_DRESS_EQUIP, ack);
    }

    private void OnReq_UseItem(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqUseItem req = Serializer.Deserialize<ReqUseItem>(ms);

        AckUseItem ack = new AckUseItem();
        ack.Num = req.Num;
        ack.Pos = req.Pos;
        ack.PosType = req.PosType;
        ack.ID = req.ID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_USEITEM, ack);
    }

    private void OnReq_ComposeChip(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqComposeChip req = Serializer.Deserialize<ReqComposeChip>(ms);

        AckComposeChip ack = new AckComposeChip();
        ack.SrcPos = req.SrcPos;
        ack.Num = req.Num;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_COMPOSE_CHIP, ack);
    }

    private void OnReq_AddHeroExp(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqAddPlayerExp req = Serializer.Deserialize<ReqAddPlayerExp>(ms);

        AckAddPlayerExp ack = new AckAddPlayerExp();
        ack.Exp = req.Exp;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_ADDHERO_EXP, ack);
    }

    private void OnReq_BuyStore(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqBuyStore req = Serializer.Deserialize<ReqBuyStore>(ms);

        AckBuyStore ack = new AckBuyStore();
        ack.StoreID     = req.StoreID;
        ack.Num         = req.Num;
        ack.StoreType   = req.StoreType;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_BUY_STORE, ack);
    }

    private void OnReq_StrengthGem(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqStrengthGem req = Serializer.Deserialize<ReqStrengthGem>(ms);

        AckStrengthGem ack = new AckStrengthGem();
        ack.TarGem = req.TarGem;
        ack.UseItems.AddRange(req.UseItems);
        NetworkManager.Instance.Send(MessageID.MSG_ACK_STRENGTHEN_GEM, ack);
    }

    private void OnReq_UpStarEquip(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqUpStarEquip req = Serializer.Deserialize<ReqUpStarEquip>(ms);

        AckUpStarEquip ack = new AckUpStarEquip();
        ack.TarEquip = req.TarEquip;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_UPSTAR_EQUIP, ack);
    }

    private void OnReq_AdvanceEquip(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqAdvanceEquip req = Serializer.Deserialize<ReqAdvanceEquip>(ms);

        DEquip        cfg = ReadCfgEquip.GetDataById(req.TarEquip.Id);
        int advanceID = cfg.Quality * 1000 + req.TarEquip.AdvanceLevel + 1;
        DEquipAdvanceCost db = ReadCfgEquipAdvanceCost.GetDataById(advanceID);

        AckAdvanceEquip ack = new AckAdvanceEquip();
        ack.UseItems.AddRange(req.UseItems);
        ack.TarEquip = req.TarEquip;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_ADVANCE_EQUIP, ack);
    }

    private void OnReq_StrengthEquip(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqStrengthEquip req = Serializer.Deserialize<ReqStrengthEquip>(ms);

        AckStrengthEquip ack = new AckStrengthEquip();
        ack.TarEquip = req.TarEquip;
        ack.UseItems.AddRange(req.UseItems);
        NetworkManager.Instance.Send(MessageID.MSG_ACK_STRENGTHEN_EQUIP, ack);
    }

    private void OnReq_SubmitTask(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqSubmitTask req = Serializer.Deserialize<ReqSubmitTask>(ms);

        AckSubmitTask ack = new AckSubmitTask();
        ack.ID   = req.ID;
        ack.Step = req.Step;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_SUBMIT_TASK, ack);
    }

    private void OnReq_UpgradePartner(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqUpgradePartner req = Serializer.Deserialize<ReqUpgradePartner>(ms);

        AckUpgradePartner ack = new AckUpgradePartner();
        ack.ID = req.ID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_UPGRADE_PARTNER, ack);
    }

    private void OnReq_AdvancePartner(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqAdvancePartner req = Serializer.Deserialize<ReqAdvancePartner>(ms);

        AckAdvancePartner ack = new AckAdvancePartner();
        ack.ID = req.ID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_ADVANVE_PARTNER, ack);
    }

    private void OnReq_ChangePartner(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqChangePartner req = Serializer.Deserialize<ReqChangePartner>(ms);

        AckChangePartner ack = new AckChangePartner();
        ack.ID = req.ID;
        ack.Pos = req.Pos;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_CHANGE_PARTNER, ack);
    }

    private void OnReq_UnloadPet(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqUnloadPet req = Serializer.Deserialize<ReqUnloadPet>(ms);

        AckUnloadPet ack = new AckUnloadPet();
        ack.ID = req.ID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_UNLOAD_PET, ack);
    }

    private void OnReq_BattlePet(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqBattlePet req = Serializer.Deserialize<ReqBattlePet>(ms);

        AckBattlePet ack = new AckBattlePet();
        ack.ID = req.ID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_BATTLE_PET, ack);
    }

    private void OnReq_UpgradePet(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqUpgradePet req = Serializer.Deserialize<ReqUpgradePet>(ms);

        AckUpgradePet ack = new AckUpgradePet();
        ack.ID = req.ID;
        ack.UseItems.AddRange(req.UseItems);
        NetworkManager.Instance.Send(MessageID.MSG_ACK_UPGRADE_PET, ack);
    }

    private void OnReq_UnloadRelics(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqUnloadRelics req = Serializer.Deserialize<ReqUnloadRelics>(ms);

        AckUnloadRelics ack = new AckUnloadRelics();
        ack.RelicsID = req.RelicsID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_UNLOAD_RELICS, ack);
    }

    private void OnReq_BattleRelics(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqBattleRelics req = Serializer.Deserialize<ReqBattleRelics>(ms);

        AckBattleRelics ack = new AckBattleRelics();
        ack.RelicsID = req.RelicsID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_BATTLE_RELICS, ack);
    }

    private void OnReq_UpgradeRelics(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqUpgradeRelics req = Serializer.Deserialize<ReqUpgradeRelics>(ms);

        AckUpgradeRelics ack = new AckUpgradeRelics();
        ack.RelicsID = req.RelicsID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_UPGRADE_RELICS, ack);
    }

    private void OnReq_ChargeRelics(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqChargeRelics req = Serializer.Deserialize<ReqChargeRelics>(ms);

        AckChargeRelics ack = new AckChargeRelics();
        ack.RelicsID = req.RelicsID;
        ack.Index    = req.Index;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_CHARGE_RELICS, ack);
    }

    private void OnReq_SetMount(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqSetMount req = Serializer.Deserialize<ReqSetMount>(ms);

        AckSetMount ack = new AckSetMount();
        ack.MountID = req.MountID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_SETMOUNT, ack);
    }

    private void OnReq_PassCopy(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqPassCopy req = Serializer.Deserialize<ReqPassCopy>(ms);

        AckPassCopy ack = new AckPassCopy();
        ack.CopyType = req.CopyType;
        ack.CopyID = req.CopyID;
        ack.Chapter = req.Chapter;
        ack.StarNum = req.StarNum;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_PASSCOPY, ack);
    }

    private void OnReq_BattleCheck(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqBattleCheck req = Serializer.Deserialize<ReqBattleCheck>(ms);

        AckBattleCheck ack = new AckBattleCheck();
        ack.CopyType = req.CopyType;
        ack.Chapter = req.Chapter;
        ack.CopyID = req.CopyID;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_BATTLE_CHECK, ack);
    }

    private void OnReq_GetChapterAward(MessageRecv obj, MessageRetCode retCode)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Packet.Data);
        ReqGetChapterAward req = Serializer.Deserialize<ReqGetChapterAward>(ms);

        AckGetChapterAward ack = new AckGetChapterAward();
        ack.CopyType = req.CopyType;
        ack.Chapter = req.Chapter;
        ack.Index = req.Index;
        NetworkManager.Instance.Send(MessageID.MSG_ACK_GET_CHAPTERAWARD, ack);
    }
}
