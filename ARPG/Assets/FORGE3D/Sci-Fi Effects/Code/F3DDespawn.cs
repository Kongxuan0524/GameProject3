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

public class F3DDespawn : MonoBehaviour {

    public float DespawnDelay;      // Despawn delay in ms
    public bool DespawnOnMouseUp;   // Despawn on mouse up used for beams demo
    
    AudioSource aSrc;               // Cached audio source component

    void Awake()
    {
        // Get audio source component
        aSrc = GetComponent<AudioSource>();        
    }

    // OnSpawned called by pool manager 
    public void OnSpawned()
    {        
        // Invokes despawn using timer delay
        if (!DespawnOnMouseUp)
            F3DTime.time.AddTimer(DespawnDelay, 1, DespawnOnTimer);
    }

    // OnDespawned called by pool manager 
    public void OnDespawned()
    { }

    // Run required checks for the looping audio source and despawn the game object
    public void DespawnOnTimer()
    {
        if (aSrc != null)
        {
            if (aSrc.loop)
                DespawnOnMouseUp = true;
            else
            {
                DespawnOnMouseUp = false;
                Despawn();
            }
        }
        else
        {
            Despawn();
        }
    }

    // Despawn game object this script attached to
    public void Despawn()
    {
        F3DPool.instance.Despawn(transform);
    }

    void Update()
    {
        // Despawn on mouse up        
        if (Input.GetMouseButtonUp(0))
            if(aSrc != null && aSrc.loop || DespawnOnMouseUp)                
                Despawn();       
    }
}
