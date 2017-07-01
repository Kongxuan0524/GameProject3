using UnityEngine;
using System.Collections;
using CFG;

namespace LVL
{
    public class LevelMachine : LevelElement
    {
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

        public override void Import(DCFG cfg)
        {
            LvlMachine data = cfg as LvlMachine;
            this.Id    = data.Id;
            this.Pos   = data.Pos;
            this.Euler = data.Euler;
        }

        public override DCFG Export()
        {
            LvlMachine data = new LvlMachine();
            data.Id    = this.Id;
            data.Pos   = this.Pos;
            data.Euler = this.Euler;
            return data;
        }

        public override void DrawScene()
        {
            Transform npcTrans = transform.FindChild("Machine");
            if (npcTrans != null)
            {
                DestroyImmediate(npcTrans.gameObject);
            }
            GTConfigManager.Instance.InitEditor();
            DMachine db = ReadCfgMachine.GetDataById(Id);
            if (db == null)
            {
                return;
            }
            GameObject npc = GTResourceManager.Instance.Load<GameObject>(db.Path, true);
            if (npc == null)
            {
                return;
            }
            npc.name = "Machine";
            npcTrans = npc.transform;
            npcTrans.ResetLocalTransform(transform);
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