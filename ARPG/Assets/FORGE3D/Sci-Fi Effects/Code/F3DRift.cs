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

public class F3DRift : MonoBehaviour {

    public float RotationSpeed;
    public float MorphSpeed, MorphFactor;

    Vector3 dScale;


	// Use this for initialization
	void Start () {

        dScale = transform.localScale;

	}
	
	// Update is called once per frame
	void Update () {

        transform.rotation = transform.rotation * Quaternion.Euler(0, 0, RotationSpeed * Time.deltaTime);
        transform.localScale = new Vector3(dScale.x, dScale.y, dScale.z + Mathf.Sin(Time.time * MorphSpeed) * MorphFactor);

	}
}
