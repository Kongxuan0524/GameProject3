using UnityEngine;
using System.Collections;
using CFG;
using BVT.AI;
using System.Collections.Generic;

namespace LVL
{
    public class LevelRegion : LevelElement
    {
        [LevelVariable]
        public string          Name        = string.Empty;
        [LevelVariable]
        public Color           Color       = new Color(0.00f, 1.00f, 1.00f, 0.5f);
        [LevelVariable]
        public bool            AwakeActive = false;
        [LevelVariable]
        public List<LvlEvent>  Events      = new List<LvlEvent>();


        public Callback<Collider> onTriggerEnter { get; set; }
        public Callback<Collider> onTriggerStay { get; set; }
        public Callback<Collider> onTriggerExit { get; set; }

        [HideInInspector][SerializeField]
        private GameObject   m_Body;
        [HideInInspector][SerializeField]
        private MeshRenderer m_MeshRenderer;

        private HashSet<int> m_HasTriggerEvents = new HashSet<int>();

        public override void Startup()
        {
            if(AwakeActive)
            {
                Trigger();
            }
        }

        public override void Trigger()
        {
            this.DrawScene();
            this.ActiveEvents(ELvlTriggerCondition.TYPE_AWAKE_REGION);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Import(DCFG cfg)
        {
            LvlRegion data   = cfg as LvlRegion;
            this.Id          = data.Id;
            this.Name        = data.Name;
            this.Pos         = data.Pos;
            this.Euler       = data.Euler;
            this.Scale       = data.Scale;
            this.AwakeActive = data.AwakeActive;
            this.Events.Clear();
            this.Events.AddRange(data.Events);
        }

        public override DCFG Export()
        {
            LvlRegion data   = new LvlRegion();
            data.Id          = this.Id;
            data.Name        = this.Name;
            data.Pos         = this.Pos;
            data.Euler       = this.Euler;
            data.Scale       = this.Scale;
            data.AwakeActive = this.AwakeActive;
            data.Events.AddRange(Events);
            return data;
        }

        public override bool AutoDrawInspector()
        {
            return true;
        }

        public override void DrawScene()
        {
            if (m_Body == null)
            {
                m_Body = GameObject.CreatePrimitive(PrimitiveType.Cube);
                m_Body.transform.ResetLocalTransform(transform);
                m_Body.transform.hideFlags = HideFlags.HideInHierarchy;
                m_Body.GET<BoxCollider>().enabled = false;
                NGUITools.SetLayer(m_Body, GTLayer.LAYER_UDT);
            }
            m_MeshRenderer = m_Body.GetComponent<MeshRenderer>();
            if (m_MeshRenderer == null)
            {
                return;
            }
            if (m_MeshRenderer.sharedMaterial != null)
            {
                Shader shader = Shader.Find("Custom/TranspUnlit");
                m_MeshRenderer.sharedMaterial = new Material(shader) { hideFlags = HideFlags.HideAndDontSave };
            }
            m_MeshRenderer.sharedMaterial.color = new Color(0.00f, 1.00f, 1.00f, 0.5f);
            BoxCollider bc = gameObject.GET<BoxCollider>();
            bc.isTrigger = true;
        }

        public override void DrawGUI()
        {
#if UNITY_EDITOR    
            this.Scale = UnityEditor.EditorGUILayout.Vector3Field("Scale", this.Scale);
#endif
        }

        private void ActiveEvents(ELvlTriggerCondition inputTriggerCondition)
        {
            for (int i = 0; i < Events.Count; i++)
            {
                if (m_HasTriggerEvents.Contains(i))
                {
                    continue;
                }
                LvlEvent e = Events[i];
                if (e.TriggerCondition == inputTriggerCondition)
                {
                    LvlSystem.TriggerLevelEvent(e);
                    m_HasTriggerEvents.Add(i);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (onTriggerEnter != null)
            {
                onTriggerEnter(other);
            }
            ActiveEvents(ELvlTriggerCondition.TYPE_ENTER_REGION);
        }

        private void OnTriggerStay(Collider other)
        {
            if (onTriggerStay != null)
            {
                onTriggerStay(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (onTriggerExit != null)
            {
                onTriggerExit(other);
            }
            ActiveEvents(ELvlTriggerCondition.TYPE_LEAVE_REGION);
        }
    }
}
