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

public class F3DRandomize : MonoBehaviour {

    private new Transform transform;            // Cached transform
    private Vector3 defaultScale;               // Default scale

    public bool RandomScale, RandomRotation;    // Randomize flags
    public float MinScale, MaxScale;            // Min/Max scale range
    public float MinRotation, MaxRotaion;       // Min/Max rotation range

    void Awake()
    {
        // Store transform component and default scale
        transform = GetComponent<Transform>();
        defaultScale = transform.localScale;
    }

    // Randomize scale and rotation according to the values set in the inspector
    void OnEnable()
    {
        if (RandomScale)
            transform.localScale = defaultScale * Random.Range(MinScale, MaxScale);

        if(RandomRotation)
            transform.rotation *= Quaternion.Euler(0, 0, Random.Range(MinRotation, MaxRotaion));
    }
}
