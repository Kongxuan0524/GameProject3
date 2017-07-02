using UnityEngine;
using System.Collections;

namespace BIE
{
    public enum EGuideType
    {
        TYPE_PATH                      =  1,  //路径任务  
        TYPE_GUICLICK                  =  2,  //UI点击   
        TYPE_GUIDOUBLECLICK            =  3,  //双击
        TYPE_GUIPRESS                  =  4,  //长按
        TYPE_GUISWAP                   =  5,  //滑动
        TYPE_CG                        =  6,  //播放视频
        TYPE_PLOT                      =  7,  //播放剧情
        TYPE_TIMELIMIT                 =  8,  //时间控制指引
    }

    public enum EGuideState
    {
        TYPE_NONE                      =  0,  
        TYPE_ENTER                     =  1,  //进入引导程序
        TYPE_EXECUTE                   =  2,  //引导任务执行中
        TYPE_FINISH                    =  3,  //完成引导
    }

    public enum EGuideUIOperationType
    {
        TYPE_NONE                      =  0,
        TYPE_CLICK                     =  1,
        TYPE_DOUBLECLICK               =  2,
        TYPE_PRESS                     =  3,
        TYPE_SWAP                      =  4,
    }

    public enum EGuideConditionRelation
    {
        AND                            =  0,  //所有条件都满足
        OR                             =  1   //只要一个条件满足即可
    }

    public enum EGuideCondition
    {
        CheckItemAmount                =  1,   //检查物品数量
        CheckPlayerLevel               =  2,   //检查玩家等级
        CheckPlayerVip                 =  3,   //检查玩家Vip
        CheckOpenUI                    =  4,   //检查打开UI
        CheckHideUI                    =  5,   //检查关闭UI
        CheckMonsterAppear             =  6,   //怪物出现
        CheckMonsterHP                 =  7,   //检查怪物血量
        CheckScene                     =  8,   //检查场景
        CheckWeekday                   =  9,   //检查星期数
        CheckPlayerHP                  =  10,  //检查玩家HP
        CheckPlayerHPPercent           =  11,  //检查玩家HP百分比
    }
}
