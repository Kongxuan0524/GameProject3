using UnityEngine;
using System.Collections;

namespace CFG
{
    //触发器条件之间的关系
    public enum ELvlCR
    {
        AND                = 0,
        OR                 = 1,
    }

    public enum ELvlMonsterWaveSpawn
    {
        TYPE_ALONG, //按顺序刷
        TYPE_WHOLE, //一次性全部刷
        TYPE_RADOM, //随机刷一个
    }

    public enum ELvlTrigger
    {
        TYPE_NONE,                //什么也不做
        TYPE_WAVESET       = 1,   //触发怪物波次
        TYPE_TASK          = 2,   //触发任务
        TYPE_PLOT          = 3,   //触发剧情
        TYPE_MACHINE       = 4,   //触发机关
        TYPE_BARRIER       = 5,   //触发光墙  
        TYPE_REGION        = 6,   //触发新的触发器
        TYPE_RESULT        = 7,   //触发副本结算
        TYPE_CUTSCENE      = 8,   //触发过场动画
        TYPE_PORTAL        = 9,   //触发一个传送门
        TYPE_BUFF          = 10,  //触发Buff、光环
        TYPE_MONSTEGROUP   = 11,  //触发怪物堆
        TYPE_MINEGROUP     = 12,  //触发矿石堆
        TYPE_LEVEL         = 13,  //触发技能
    }

    public enum ELvlPathType
    {
        Linear,//线性
        Bezier,//贝塞尔
    }

    public enum ELvlTriggerCondition
    {
        TYPE_AWAKE_REGION  = 0,//区域生成时触发
        TYPE_ENTER_REGION  = 1,//进入区域
        TYPE_LEAVE_REGION  = 2,//离开区域
        TYPE_WAVES_FINISH  = 3,//波次结束后触发
    }

    public enum ELvlDropObjectAbsorbMode
    {
        LineChase          = 1,    //直线追踪
        LineChaseAndCircle = 2,    //先上升，然后旋转追踪目标吸附
    }

    public enum ELvlGroup
    {
        Born,
        Barrier,
        Monster,
        Region,
        Portal,
        Npc,
        Obj,
        Machine,
        Path,
        RegionMonster,
        RegionWave,
        RegionMine,
    }


    public enum ELvlDropObjectState
    {
        None,
        Created,       //已创建
        Splash,        //四处溅射，宝物向四周随机角度抛物线弹出
        Raise,         //升高
        Wait,          //等待
        CircleFly,     //曲线飞向主角
        LineFly,       //直线飞向主角
        Dead,          //消亡
    }

    public enum ELvlObjType
    {
        Plant  = 0,
        Build  = 1,
        Grass  = 2,
    }
}

