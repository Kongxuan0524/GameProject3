using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class HUDBoard : MonoBehaviour
{
    private bool        mNeedUpdate = true;
    private float       mHeight = 0f;
    private Transform   mTarget;
    private string      mPath;
    private TextMeshPro mText;

    void Awake()
    {
        mText = transform.Find("Text").GetComponent<TextMeshPro>();
        List<Renderer> rendererList = new List<Renderer>();
        GetComponentsInChildren<Renderer>(rendererList);
        for (int i = 0; i < rendererList.Count; i++)
        {
            rendererList[i].material.renderQueue = 2500;
        }
    }

    void Update()
    {
        if (mNeedUpdate == false)
        {
            return;
        }
        if (mTarget == null)
        {
            return;
        }
        UpdatePosition(mTarget);
    }

    public void SetVisable(bool val)
    {
        gameObject.SetActive(val);
    }

    public void SetTarget(Transform target)
    {
        mTarget = target;
        UpdatePosition(target);
    }

    public void SetPath(string path)
    {
        mPath = path;
    }

    public void SetHeight(float height)
    {
        mHeight = height;
    }

    public void UpdatePosition(Transform target)
    {
        if (GTCameraManager.Instance.MainCamera == null ||
            GTCameraManager.Instance.NGUICamera == null)
        {
            return;
        }
        if (GTCameraManager.Instance.CameraCtrl == null || GTCameraManager.Instance.CameraCtrl.isShake)
        {
            return;
        }
        Vector3 pos_3d = target.position + new Vector3(0, mHeight, 0);
        Vector2 pos_screen = GTCameraManager.Instance.MainCamera.WorldToScreenPoint(pos_3d);
        Vector3 pos_ui = GTCameraManager.Instance.NGUICamera.ScreenToWorldPoint(pos_screen);
        transform.position = Vector3.Slerp(transform.position, pos_ui, Time.time * 5);
    }

    public void Show(string text1, string text2 = null, int titleID = 0)
    {
        string text = string.Empty;
        if (string.IsNullOrEmpty(text1) == false)
        {
            text = text1;
        }
        if (string.IsNullOrEmpty(text2) == false)
        {
            text = text +'\n' + text2;
        }
        mText.text = text;
    }

    public void Release()
    {
        this.enabled = false;
        GTPoolManager.Instance.ReleaseGo(mPath, gameObject);
    }
}
