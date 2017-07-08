using UnityEngine;
using System.Collections;


public enum EWindowType
{
    BOTTOM,
    FLYWORD,
    FIXED,
    WINDOW,
    DIALOG,
    MSGTIP,
    LOADED,
    MASKED,
}

public enum EDialogType
{
    SureToBuy,
}

public enum EWindowID
{
    UILogin,
    UIAccount,
    UIServer,
    UINotice,

    UINetWaiting,
    UILoading,
    UIMask,

    UISetting,

    UIMessageTip,
    UIMessageBox,
    UIMessageBoxForNetwork,
    UIAwardTip,
    UIAwardBox,

    UICreateRole,
    UIHome,
    UIBag,
    UIEquip,
    UIGem,
    UIPet,
    UIStore,
    UIWorldMap,
    UIAdventure,
    UISkill,
    UIReborn,

    UITask,
    UITaskTalk,
    UITaskInterActive,

    UIHeroInfo,
    UIItemInfo,
    UIItemUse,
    UIEquipInfo,
    UIGemInfo,
    UIChipInfo,
    UIFashionInfo,
    UIRuneInfo,

    UIRoleEquip,
    UIRoleGem,
    UIRoleFashion,
    UIRoleRune,
    UIRoleFetter,

    UIMainRaid,
    UIMainGate,
    UIMainResult,
    UIMainBossHP,



    UIPartner,
    UIPartnerAdvance,
    UIPartnerStrength,
    UIPartnerStar,
    UIPartnerWake,
    UIPartnerWash,
    UIPartnerSkill,
    UIPartnerFetter,
    UIPartnerProperty,
    UIPartnerBattle,

    UIMount,
    UIMountLibrary,
    UIMountBlood,
    UIMountTame,
    UIMountTurned,

    UIRelics,
    UIRelicsSkill,

    UIGuide,

    UIPlotCutscene,
}

public enum EWindowMaskType
{
    None,
    BlackTransparent,
    WhiteTransparent,
    Blur,
    Black,
}

public enum EWindowHideType
{
    Normal,
    Scale,
}

public enum EWindowOpenType
{
    Normal,
    Scale,
}