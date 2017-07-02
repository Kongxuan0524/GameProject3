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

public class F3DBurnoutExample : MonoBehaviour {

    MeshRenderer[] turretParts;
    int BurnoutID;


	// Use this for initialization
	void Start () {

        BurnoutID = Shader.PropertyToID("_BurnOut");
        turretParts = GetComponentsInChildren<MeshRenderer>();

	}
	
	// Update is called once per frame
	void Update () {
	
        for(int i = 0; i < turretParts.Length; i++)
        {
            turretParts[i].material.SetFloat(BurnoutID, Mathf.Lerp(0, 2f, (Mathf.Sin(Time.time)) / 2));
        }

	}
}
