using UnityEngine;
using System.Collections;
using CFG;

namespace LVL
{
    public class LevelBarrier : LevelElement
    {
        [LevelVariable]
        public float Width = 14;

        public override void Startup()
        {

        }

        public override void Trigger()
        {
            this.DrawScene();
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Import(DCFG cfg)
        {
            LvlBarrier data = cfg as LvlBarrier;
            this.Id         = data.Id;
            this.Width      = data.Width;
            this.Pos        = data.Pos;
            this.Euler      = data.Euler;
        }

        public override DCFG Export()
        {
            LvlBarrier data = new CFG.LvlBarrier();
            data.Id         = this.Id;
            data.Width      = this.Width;
            data.Pos        = this.Pos;
            data.Euler      = this.Euler;
            return data;
        }

        public override void DrawScene()
        {
            float width = Width < 1 ? 14 : Width;
            int count = Mathf.CeilToInt(Width / 14);
            Vector3 size = Vector3.zero;
            size.x = count * 14;
            size.y = 10;
            size.z = 1.5f;

            Transform body = transform.FindChild("Body");
            if (body == null)
            {
                body = new GameObject("Body").transform;
                body.parent = transform;
                body.transform.localPosition = Vector3.zero;
                body.localEulerAngles = Vector3.zero;
                body.hideFlags = HideFlags.HideInHierarchy;
            }
            else
            {
                NGUITools.DestroyChildren(body);
            }
            float halfCount = count * 0.5f;
            for (int i = 0; i < count; i++)
            {
                GameObject unit = GTResourceManager.Instance.Instantiate(GTPrefabKey.PRE_BARRIER);
                if (unit == null)
                {
                    return;
                }
                unit.name = i.ToString();
                Transform trans = unit.transform;
                Vector3 localPosition = Vector3.right * (i - halfCount + 0.5f) * 14;
                localPosition.z = size.z * 0.5f;
                trans.localPosition = localPosition;
                trans.SetParent(body, false);
            }
            BoxCollider bc = gameObject.GET<BoxCollider>();
            bc.size = size;
            bc.center = new Vector3(0, size.y * 0.5f - 1f, size.z * 0.5f);
            bc.hideFlags = HideFlags.HideInInspector;
            NGUITools.SetLayer(gameObject, GTLayer.LAYER_BARRER);
        }

        public override void DrawGUI()
        {
#if UNITY_EDITOR
            int id = UnityEditor.EditorGUILayout.IntField("Id", Id);
            if (id != Id)
            {
                this.Id = id;
                this.SetName();
            }
            float w = UnityEditor.EditorGUILayout.FloatField("Width", Width);
            if (w != Width)
            {
                this.Width = w;
                this.DrawScene();
            }
#endif
        }
    }
}