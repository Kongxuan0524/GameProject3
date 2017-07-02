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

public class F3DWarpTunnel : MonoBehaviour {

    public float MaxRotationSpeed;
    public float AdaptationFactor;
    

    float speed, newSpeed;
	// Use this for initialization
	void Start () {
        speed = 0;
        OnDirectionChange();        
	}
	
    void OnDirectionChange()
    {
        newSpeed = Random.Range(-MaxRotationSpeed, MaxRotationSpeed);
        F3DTime.time.AddTimer(Random.Range(1, 5), 1, OnDirectionChange);
    }

	// Update is called once per frame
	void Update () {

        speed = Mathf.Lerp(speed, newSpeed, Time.deltaTime * AdaptationFactor);
        transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * Quaternion.Euler(speed, 0, 0), Time.deltaTime);
	}
}
