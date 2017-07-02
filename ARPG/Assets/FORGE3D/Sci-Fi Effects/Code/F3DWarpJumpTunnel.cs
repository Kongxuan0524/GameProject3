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

public class F3DWarpJumpTunnel : MonoBehaviour
{

    private new Transform transform;
    private MeshRenderer meshRenderer;

    public float StartDelay, FadeDelay;
    public Vector3 ScaleTo;
    public float ScaleTime;
    public float ColorTime, ColorFadeTime;
    public float RotationSpeed;

    private bool grow = false;

    float alpha;

    int alphaID;

    void Awake()
    {
        transform = GetComponent<Transform>();
        meshRenderer = GetComponent<MeshRenderer>();

        alphaID = Shader.PropertyToID("_Alpha");
    }

    public void OnSpawned()
    {
        grow = false;      
        meshRenderer.material.SetFloat(alphaID, 0);
        F3DTime.time.AddTimer(StartDelay, 1, ToggleGrow);
        F3DTime.time.AddTimer(FadeDelay, 1, ToggleGrow);
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.localRotation = transform.localRotation * Quaternion.Euler(0, 0, Random.Range(-360, 360));
    }

    void ToggleGrow()
    {
        grow = !grow;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, RotationSpeed * Time.deltaTime);

        if (grow)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, ScaleTo, Time.deltaTime * ScaleTime);

            alpha = Mathf.Lerp(alpha, 1, Time.deltaTime * ColorTime);
            meshRenderer.material.SetFloat(alphaID, alpha);
        }
        else
        {
            alpha = Mathf.Lerp(alpha, 0, Time.deltaTime * ColorFadeTime);
            meshRenderer.material.SetFloat(alphaID, alpha);
        }
    }
}
