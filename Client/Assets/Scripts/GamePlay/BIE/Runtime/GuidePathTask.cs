using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;

namespace BIE
{
    public class GuidePathTask : GuideBase
    {
        public const string RESPATH_TARGETARROW = "Effect/other_xinshou_yidong";
        public const string RESPATH_ARROW       = "Effect/jiantourow";
        public const int    MAXCOUNT            = 20;

        public Vector3 TargetScale      = new Vector3(5, 5, 5);
        public Vector3 TargetPos        = new Vector3(0, -2.16f, 0);
        public string  TargetEffectPath = string.Empty;

        private Character         m_Character;
        private Vector3           m_DirectionToTarget;
        private ETriggerObject    m_TargetTriggerObject;
        private GameObject        m_TargetArrow;
        private List<GameObject>  m_ArrowGameObjectList = new List<GameObject>();
        private Queue<GameObject> m_ArrowQueuePool = new Queue<GameObject>();

        public override void Enter()
        {
            base.Enter();
            m_Character = CharacterManager.Main;
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.GetComponent<Collider>().isTrigger = true;
            m_TargetTriggerObject = go.AddComponent<ETriggerObject>();
            m_TargetTriggerObject.transform.localScale = TargetScale;
            m_TargetTriggerObject.transform.localPosition = TargetPos;
            m_TargetTriggerObject.GetComponent<MeshRenderer>().enabled = false;
            m_TargetTriggerObject.onTriggerEnter = OnTriggerEnter;
            m_TargetArrow = GTResourceManager.Instance.Load<GameObject>(RESPATH_TARGETARROW, true);
            m_TargetArrow.transform.localPosition = TargetPos;
            for (int i = 0; i < MAXCOUNT; i++)
            {
                GameObject g = GTResourceManager.Instance.Load<GameObject>(RESPATH_ARROW, true);
                m_ArrowQueuePool.Enqueue(g);
            }
        }

        public override void Execute()
        {
            base.Execute();
        }

        public override void Finish()
        {
            m_TargetTriggerObject.gameObject.SetActive(false);
            m_TargetArrow.gameObject.SetActive(false);
            base.Finish();
        }

        public override void Stop()
        {
            m_TargetTriggerObject.gameObject.SetActive(false);
            m_TargetArrow.gameObject.SetActive(false);
            base.Stop();
        }

        public          void UpdateArrowList()
        {

        }

        public override void Read(XmlElement os)
        {
            base.Read(os);
            this.TargetScale       = os.GetVector3("TargetScale");
            this.TargetPos         = os.GetVector3("TargetPos");
            this.TargetEffectPath  = os.GetString("TargetEffectPath");
        }

        private void OnTriggerEnter(Collider obj)
        {
            if (obj.gameObject == null)
            {
                return;
            }
            CharacterView view = obj.gameObject.GetComponent<CharacterView>();
            if (view == null)
            {
                return;
            }
            if(m_Character == view.Owner)
            {
                Finish();
            }
        }
    }  
}

