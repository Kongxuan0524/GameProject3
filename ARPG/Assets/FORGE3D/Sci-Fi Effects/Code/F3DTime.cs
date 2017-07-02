/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class F3DTime : MonoBehaviour
{
    public static F3DTime time;

    // Timer objects
    List<Timer> timers;
    // Timer removal queue
    List<int> removalPending;

    private int idCounter;

    /// <summary>
    /// Timer entity class
    /// </summary>
    class Timer
    {
        public int id;
        public bool isActive;

        public float rate;
        public int ticks;
        public int ticksElapsed;
        public float last;
        public Action callBack;

        public Timer(int id_, float rate_, int ticks_, Action callback_)
        {
            id = id_;
            rate = rate_ < 0 ? 0 : rate_;
            ticks = ticks_ < 0 ? 0 : ticks_;
            callBack = callback_;
            last = 0;
            ticksElapsed = 0;
            isActive = true;
        }

        public void Tick()
        {
            last += Time.deltaTime;

            if (isActive && last >= rate)
            {
                last = 0;
                ticksElapsed++;
                callBack.Invoke();

                if (ticks > 0 && ticks == ticksElapsed)
                {
                    isActive = false;
                    F3DTime.time.RemoveTimer(id);
                }
            }
        }
    }

    /// <summary>
    /// Awake
    /// </summary>
    void Awake()
    {
        time = this;
        timers = new List<Timer>();
        removalPending = new List<int>();
    }

    /// <summary>
    /// Creates new timer
    /// </summary>
    /// <param name="rate">Tick rate</param>
    /// <param name="callBack">Callback method</param>
    /// <returns>Time GUID</returns>
    public int AddTimer(float rate, Action callBack)
    {
        return AddTimer(rate, 0, callBack);
    }

    /// <summary>
    /// Creates new timer
    /// </summary>
    /// <param name="rate">Tick rate</param>
    /// <param name="ticks">Number of ticks before timer removal</param>
    /// <param name="callBack">Callback method</param>
    /// <returns>Timer GUID</returns>
    public int AddTimer(float rate, int ticks, Action callBack)
    {
        Timer newTimer = new Timer(++idCounter, rate, ticks, callBack);
        timers.Add(newTimer);
        return newTimer.id;
    }

    /// <summary>
    /// Removes timer
    /// </summary>
    /// <param name="timerId">Timer GUID</param>
    public void RemoveTimer(int timerId) { removalPending.Add(timerId); }

    /// <summary>
    /// Timer removal queue handler
    /// </summary>
    void Remove()
    {
        if (removalPending.Count > 0)
        {
            foreach (int id in removalPending)
                for (int i = 0; i < timers.Count; i++)
                    if (timers[i].id == id)
                    {
                        timers.RemoveAt(i);
                        break;
                    }

            removalPending.Clear();
        }
    }

    /// <summary>
    /// Updates timers
    /// </summary>
    void Tick()
    {
        for (int i = 0; i < timers.Count; i++)
            timers[i].Tick();
    }

    // Update is called once per frame
    void Update()
    {
        Remove();
        Tick();
    }
}
