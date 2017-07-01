using UnityEngine;
using System.Collections;
using CFG;

namespace LVL
{
    public class LevelMonster: LevelElement
    {
        public override void Startup()
        {

        }

        public override void Trigger()
        {

        }

        public override void Release()
        {

        }

        public override void Import(DCFG cfg)
        {
            LvlMonster data = cfg as LvlMonster;
            this.Id         = data.Id;
            this.Pos        = data.Pos;
            this.Euler      = data.Euler;
        }

        public override DCFG Export()
        {
            LvlMonster data = new LvlMonster();
            data.Id         = this.Id;
            data.Pos        = this.Pos;
            data.Euler      = this.Euler;
            return data;
        }

        public          void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(Pos, Vector3.one);
        }

        public override void DrawScene()
        {

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
