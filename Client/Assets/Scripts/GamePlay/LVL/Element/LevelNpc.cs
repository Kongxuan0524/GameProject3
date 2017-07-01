using UnityEngine;
using System.Collections;
using CFG;
using System.Collections.Generic;

namespace LVL
{
    public class LevelNpc : LevelElement
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
            LvlNpc data = cfg as LvlNpc;
            this.Id         = data.Id;
            this.Pos        = data.Pos;
            this.Euler      = data.Euler;
        }

        public override DCFG Export()
        {
            LvlNpc data     = new LvlNpc();
            data.Id         = this.Id;
            data.Pos        = this.Pos;
            data.Euler      = this.Euler;
            return data;
        }

        public override void DrawScene()
        {
            Transform npcTrans = transform.FindChild("Npc");
            if (npcTrans != null)
            {
                DestroyImmediate(npcTrans.gameObject);
            }
            GTConfigManager.Instance.InitEditor();
            DActor db = ReadCfgActor.GetDataById(Id);
            if (db == null)
            {
                return;
            }
            DActorModel dbModel = ReadCfgActorModel.GetDataById(db.Model);
            GameObject npc = GTResourceManager.Instance.Load<GameObject>(dbModel.Model, true);
            if (npc == null)
            {
                return;
            }
            npc.name = "Npc";
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