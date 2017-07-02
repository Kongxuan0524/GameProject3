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

public class F3DTrailExample : MonoBehaviour
{
    public float Mult;
    public float TimeMult;

    Vector3 defaultPos;

    // Use this for initialization
    void Start ()
    {
        // Store initial position
        defaultPos = transform.position;
    }
    
    // Update is called once per frame
    void Update ()
    {
        // Used in the example scene
        // Moves the trail by circular trajectory 
        transform.position = defaultPos + new Vector3(Mathf.Sin(Time.time * TimeMult) * Mult, 0f, Mathf.Cos(Time.time * TimeMult) * Mult);    
    }
}
