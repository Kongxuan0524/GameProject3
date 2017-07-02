using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CFG;

namespace LVL
{
    public class LevelPath : LevelElement
    {
        [LevelVariable]
        public ELvlPathType         Type      = ELvlPathType.Linear;
        [LevelVariable]
        public List<LvlPathNode>    PathNodes = new List<LvlPathNode>();

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
            LvlPath data   = cfg as LvlPath;
            this.Id        = data.Id;
            this.Type      = data.Type;
            this.PathNodes = data.PathNodes;
        }

        public override DCFG Export()
        {
            LvlPath data   = new LvlPath();
            data.Id        = this.Id;
            data.Type      = this.Type;
            data.PathNodes = this.PathNodes;
            return data;
        }

        public override void DrawScene()
        {
            base.DrawScene();
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
            this.Type = (ELvlPathType)UnityEditor.EditorGUILayout.EnumPopup("Type", Type);
            GUI.color = Color.green;
            GUILayout.Space(10);
            if (GUILayout.Button("Add Node", GUILayout.Height(30)))
            {
                LvlPathNode node = new LvlPathNode();
                PathNodes.Add(node);
            }
            GUI.color = Color.white;
            GUILayout.Space(5);
            for (int i = PathNodes.Count - 1; i >= 0; i--)
            {
                GUILayout.Space(5);
                UnityEditor.EditorGUILayout.LabelField("路径点" + i, BVT.BTStyle.STYLE_CENTERLABEL, GUILayout.Height(30));
                PathNodes[i].Pos = UnityEditor.EditorGUILayout.Vector3Field("Pos", PathNodes[i].Pos);
                PathNodes[i].Euler = UnityEditor.EditorGUILayout.Vector3Field("Euler", PathNodes[i].Euler);
                GUI.color = Color.red;
                if (GUILayout.Button("X", GUILayout.Width(40)))
                {
                    PathNodes.RemoveAt(i);
                }
                GUI.color = Color.white;
            }
#endif
        }

        public void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (PathNodes.Count < 2)
            {
                return;
            }
            for (int i = 0; i < PathNodes.Count; i++)
            {
                LvlPathNode node1 = PathNodes[i];
                if (i <= PathNodes.Count - 2)
                {
                    LvlPathNode node2 = PathNodes[i + 1];
                    UnityEditor.Handles.color = Color.green;

                    switch(Type)
                    {
                        case ELvlPathType.Linear:
                            UnityEditor.Handles.DrawLine(node1.Pos, node2.Pos);
                            break;
                        case ELvlPathType.Bezier:
                            UnityEditor.Handles.DrawLine(node1.Pos, node2.Pos);
                            break;
                    }      
                }
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(node1.Pos, 0.2f);
                UnityEditor.Handles.Label(node1.Pos + new Vector3(0, 0.5f, 0), string.Format("<color=#00ff00>{0}</color>", i), BVT.BTStyle.STYLE_CENTERLABEL);
            }
#endif
        }
    }
}
