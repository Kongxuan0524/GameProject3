using UnityEngine;
using System.Collections;
using System;
using BIE;

public class UIGuide : GTWindow
{
    private GameObject m_Mask;
    private Transform  m_Pivot;
    private Transform  m_Hand;
    private UISprite   m_AreaCircle;
    private UISprite   m_AreaRect;
    private UISprite   m_AreaRow;
    private Transform  m_TipTransform;
    private UILabel    m_TipText;

    public UIGuide()
    {
        mResPath = "Guide/UIGuide";
        mResident = false;
        Type = EWindowType.MASKED;
    }

    protected override void OnAwake()
    {
        m_Mask          = transform.Find("Mask").gameObject;
        m_Pivot         = transform.Find("Pivot");
        m_Hand          = m_Pivot.Find("Hand");
        m_AreaCircle    = m_Pivot.Find("Area_Circle").GetComponent<UISprite>();
        m_AreaRect      = m_Pivot.Find("Area_Rect").GetComponent<UISprite>();
        m_AreaRow       = m_Pivot.Find("Area_Row").GetComponent<UISprite>();
        m_TipTransform  = m_Pivot.Find("Tip");
        m_TipText       = m_TipTransform.Find("Content").GetComponent<UILabel>();
        m_Hand.gameObject.SetActive(false);
        m_AreaCircle.gameObject.SetActive(false);
        m_AreaRect.gameObject.SetActive(false);
        m_AreaRow.gameObject.SetActive(false);
        m_TipTransform.gameObject.SetActive(false);
        m_TipText.text = string.Empty;
    }

    protected override void OnEnable()
    {
        
    }

    protected override void OnAddButtonListener()
    {
        
    }

    protected override void OnAddHandler()
    {
        
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnDelHandler()
    {
        
    }

    public void ShowViewByGuideBaseData(GuideBase data)
    {
        m_Mask.SetActive(data.IsLocked);
        if(string.IsNullOrEmpty(data.TipText))
        {
            m_TipTransform.gameObject.SetActive(false);
        }
        else
        {
            m_TipText.text = data.TipText;
            m_TipTransform.gameObject.SetActive(true);
            m_TipTransform.transform.localPosition = data.TipPosition;
        }
    }

    public void DoGuideForClick(Transform target, GuideUIClick data)
    {
        GameObject btn = null;
        DoGuideInstantie(target, ref btn);
        UIEventListener.Get(btn).onClick = (go) =>
        {
            UIEventListener ul = target.GetComponent<UIEventListener>();
            if (ul != null && ul.onClick != null)
            {
                ul.onClick(go);
            }
            data.Finish();
            UnityEngine.GameObject.DestroyImmediate(btn);
        };
    }

    public void DoGuideForDoubleClick(Transform target, GuideUIDoubleClick data)
    {
        GameObject btn = null;
        DoGuideInstantie(target, ref btn);
        UIEventListener.Get(btn).onDoubleClick = (go) =>
        {
            UIEventListener ul = target.GetComponent<UIEventListener>();
            if (ul != null && ul.onDoubleClick != null)
            {
                ul.onDoubleClick(go);
            }
            data.Finish();
            UnityEngine.GameObject.DestroyImmediate(btn);
        };
    }

    public void DoGuideInstantie(Transform target ,ref GameObject btn)
    {
        if (target == null)
        {
            return;
        }
        BoxCollider collider = target.GetComponent<BoxCollider>();
        if (collider == null)
        {
            return;
        }
        btn = NGUITools.AddChild(transform.gameObject, target.gameObject);
        btn.transform.position = target.transform.position;
    }

    public void DoGuideForPress(Transform target, GuideUIPress data)
    {
        GameObject btn = null;
        DoGuideInstantie(target, ref btn);
        UIEventListener.Get(btn).onPress = (go, state) =>
        {
            UIEventListener ul = target.GetComponent<UIEventListener>();
            if (ul != null && ul.onPress != null)
            {
                ul.onPress(go, state);
            }
            data.Finish();
            UnityEngine.GameObject.DestroyImmediate(btn);
        };
    }

    public void DoGuideForSwap(Transform target, GuideUISwap data)
    {
        GameObject btn = null;
        DoGuideInstantie(target, ref btn);
        UIEventListener.Get(btn).onDrag = (go, delta) =>
        {
            UIEventListener ul = target.GetComponent<UIEventListener>();
            if (ul != null && ul.onDrag != null)
            {
                ul.onDrag(go, delta);
            }
            data.Finish();
            UnityEngine.GameObject.DestroyImmediate(btn);
        };
    }
}
