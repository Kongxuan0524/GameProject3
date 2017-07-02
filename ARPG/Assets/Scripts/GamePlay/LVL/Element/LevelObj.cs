using UnityEngine;
using System.Collections;
using CFG;

namespace LVL
{
    public class LevelObj : LevelElement
    {
        [LevelVariable]
        public ELvlObjType Type = ELvlObjType.Build;

        public override void Startup()
        {

        }

        public override void Trigger()
        {
            this.DrawScene();
        }

        public override void Release()
        {

        }

        public override void Import(DCFG cfg)
        {
            LvlObj data = cfg as LvlObj;
            this.Id     = data.Id;
            this.Type   = data.Type;
            this.Pos    = data.Pos;
            this.Euler  = data.Euler;
        }

        public override DCFG Export()
        {
            LvlObj data = new LvlObj();
            data.Id     = this.Id;
            data.Type   = this.Type;
            data.Pos    = this.Pos;
            data.Euler  = this.Euler;
            return data;
        }

        public override void DrawScene()
        {
            Transform objTrans = transform.FindChild("SceneObj");
            if (objTrans != null)
            {
                DestroyImmediate(objTrans.gameObject);
            }
            GTConfigManager.Instance.InitEditor();
            DSceneObj db = ReadCfgSceneObj.GetDataById(Id);
            if (db == null)
            {
                return;
            }
            GameObject npc = GTResourceManager.Instance.Load<GameObject>(db.Path, true);
            if (npc == null)
            {
                return;
            }
            npc.name = "SceneObj";
            objTrans = npc.transform;
            objTrans.ResetLocalTransform(transform);
        }

        public override void DrawGUI()
        {
#if UNITY_EDITOR
            int id = UnityEditor.EditorGUILayout.IntField("Id", Id);
            if (id != Id)
            {
                this.Id = id;
                this.DrawScene();
                this.SetName();
            }
#endif
        }
    }
}
