using UnityEngine;
using System.Collections;
using CFG;

namespace LVL
{
    public class LevelBorn : LevelElement
    {
        [LevelVariable]
        public EBattleCamp Camp = EBattleCamp.A;

        [SerializeField]
        private GameObject   m_Body;
        [SerializeField]
        private MeshRenderer m_MeshRenderer;
        [SerializeField]
        private Material     m_Material;
        [SerializeField]
        private EBattleCamp  m_CurCamp = EBattleCamp.A;

        public override void Startup()
        {

        }

        public override void Trigger()
        {
            
        }

        public override void Release()
        {
            base.Release();
        }

        public override void SetName()
        {
            this.name = GTTools.Format("{0}_{1}", this.GetType().Name, Camp);
        }

        public override void Import(DCFG cfg)
        {
            LvlBorn data = cfg as LvlBorn;
            this.Camp    = data.Camp;
            this.Pos     = data.Pos;
            this.Euler   = data.Euler;
        }

        public override DCFG Export()
        {
            LvlBorn data = new LvlBorn();
            data.Camp    = this.Camp;
            data.Pos     = this.Pos;
            data.Euler   = this.Euler;
            return data;
        }

        public override void DrawScene()
        {
            if (m_Body == null)
            {
                m_Body = GameObject.CreatePrimitive(PrimitiveType.Cube);
                m_Body.transform.ResetLocalTransform(transform);
                m_Body.transform.hideFlags = HideFlags.HideInHierarchy;
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
            switch (Camp)
            {
                case EBattleCamp.A:
                    m_MeshRenderer.sharedMaterial.color = new Color(1.00f, 0.00f, 0.00f, 0.5f);
                    break;
                case EBattleCamp.B:
                    m_MeshRenderer.sharedMaterial.color = new Color(0.00f, 1.00f, 1.00f, 0.5f);
                    break;
                case EBattleCamp.C:
                    m_MeshRenderer.sharedMaterial.color = new Color(1.00f, 0.92f, 0.02f, 0.5f);
                    break;
            }
        }

        public override void DrawGUI()
        {
#if UNITY_EDITOR
            m_CurCamp = (EBattleCamp)UnityEditor.EditorGUILayout.EnumPopup("Camp", Camp);
            if (m_CurCamp != Camp)
            {
                this.Camp = m_CurCamp;
                this.DrawScene();
                this.SetName();
            }
#endif
        }
    }
}